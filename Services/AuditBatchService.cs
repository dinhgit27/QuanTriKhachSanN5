using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Services
{
    public class AuditBatchService : IAuditBatchService
    {
        private readonly ApplicationDbContext _context;

        public AuditBatchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddEventAsync(int userId, string role, object eventObj)
        {
            try
            {
                // Lấy ngày hiện tại (không lấy giờ)
                var today = DateTime.UtcNow.Date;
                var tomorrow = today.AddDays(1);

                // Tìm record log của user này trong ngày hôm nay
                // Dùng range so sánh thay vì .Date để tránh lỗi EF Core translation
                var auditLog = await _context.AuditLogs.FirstOrDefaultAsync(l =>
                    l.UserId == userId && l.Timestamp >= today && l.Timestamp < tomorrow
                );

                AuditLogPayload payload;

                if (auditLog == null)
                {
                    // Nếu chưa có, tạo mới
                    payload = new AuditLogPayload
                    {
                        TotalEvents = 1,
                        Events = new List<object> { eventObj },
                    };

                    auditLog = new Audit_Log
                    {
                        UserId = userId,
                        RoleName = role,
                        Timestamp = DateTime.UtcNow,
                        LogData = JsonSerializer.Serialize(payload),
                    };

                    _context.AuditLogs.Add(auditLog);
                }
                else
                {
                    // Nếu đã có, giải mã JSON cũ và thêm event mới vào
                    try
                    {
                        payload =
                            JsonSerializer.Deserialize<AuditLogPayload>(auditLog.LogData)
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
            catch (Exception ex)
            {
                // Không để lỗi audit làm gián đoạn luồng nghiệp vụ chính
                Console.WriteLine($"[AuditBatch] Lỗi ghi audit log: {ex.Message}");
            }
        }
    }

    // Class phụ trợ để Map dữ liệu JSON
    public class AuditLogPayload
    {
        public int TotalEvents { get; set; }
        public List<object> Events { get; set; } = new List<object>();
    }
}
