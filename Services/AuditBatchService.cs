using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Models;
using QuanTriKhachSanN5.Interfaces;

namespace QuanTriKhachSanN5.Services
{
    public class AuditBatchService : IAuditBatchService
    {
        private readonly ApplicationDbContext _context;

        public AuditBatchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddEventAsync(int? userId, string role, object eventObj)
        {
            // Lấy ngày hiện tại (không lấy giờ)
            var today = DateTime.UtcNow.Date;

            // Tìm record log của user này (hoặc system nếu userId null) trong ngày hôm nay
            var auditLog = await _context.AuditLogs
                .FirstOrDefaultAsync(l => l.UserId == userId && l.Timestamp.Date == today);

            AuditLogPayload payload;

            if (auditLog == null)
            {
                // Nếu chưa có, tạo mới
                payload = new AuditLogPayload
                {
                    TotalEvents = 1,
                    Events = new List<object> { eventObj }
                };

                auditLog = new Audit_Log
                {
                    UserId = userId,
                    RoleName = role,
                    Timestamp = DateTime.UtcNow,
                    LogData = JsonSerializer.Serialize(payload)
                };

                _context.AuditLogs.Add(auditLog);
            }
            else
            {
                // Nếu đã có, giải mã JSON cũ và thêm event mới vào
                try 
                {
                    payload = JsonSerializer.Deserialize<AuditLogPayload>(auditLog.LogData) 
                             ?? new AuditLogPayload();
                }
                catch
                {
                    payload = new AuditLogPayload();
                }

                payload.Events.Add(eventObj);
                payload.TotalEvents = payload.Events.Count;

                auditLog.LogData = JsonSerializer.Serialize(payload);
                _context.AuditLogs.Update(auditLog);
            }

            await _context.SaveChangesAsync();
        }
    }

    // Class phụ trợ để Map dữ liệu JSON với format lowercase
    public class AuditLogPayload
    {
        [System.Text.Json.Serialization.JsonPropertyName("totalEvents")]
        public int TotalEvents { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("events")]
        public List<object> Events { get; set; } = new List<object>();
    }
}
