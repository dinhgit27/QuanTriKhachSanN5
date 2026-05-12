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
                // Parse properties dynamically from incoming payload (often anonymous objects or DTOs)
                string action = "OTHER";
                string tableName = "System";
                int? recordId = null;
                string oldValue = "{}";
                string newValue = "{}";

                if (eventObj != null)
                {
                    var options = new JsonSerializerOptions { WriteIndented = false };
                    var jsonString = JsonSerializer.Serialize(eventObj, options);
                    using var doc = JsonDocument.Parse(jsonString);
                    var root = doc.RootElement;

                    if (root.TryGetProperty("actionType", out var actProp))
                        action = actProp.GetString() ?? "OTHER";
                    else if (root.TryGetProperty("action", out var actProp2))
                        action = actProp2.GetString() ?? "OTHER";

                    if (root.TryGetProperty("module", out var modProp))
                        tableName = modProp.GetString() ?? "System";
                    else if (root.TryGetProperty("targetTable", out var tblProp))
                        tableName = tblProp.GetString() ?? "System";

                    if (root.TryGetProperty("recordId", out var recProp))
                    {
                        if (recProp.ValueKind == JsonValueKind.Number)
                            recordId = recProp.GetInt32();
                        else if (recProp.ValueKind == JsonValueKind.String && int.TryParse(recProp.GetString(), out int parsedId))
                            recordId = parsedId;
                    }

                    if (root.TryGetProperty("oldValue", out var oldProp))
                        oldValue = oldProp.ValueKind == JsonValueKind.String ? (oldProp.GetString() ?? "{}") : oldProp.ToString();
                    if (root.TryGetProperty("newValue", out var newProp))
                        newValue = newProp.ValueKind == JsonValueKind.String ? (newProp.GetString() ?? "{}") : newProp.ToString();
                    else if (root.TryGetProperty("description", out var descProp))
                        newValue = descProp.GetString() ?? "{}";
                }

                var auditLog = new Audit_Log
                {
                    UserId = userId == 0 ? (int?)null : userId,
                    Action = action,
                    TableName = tableName,
                    RecordId = recordId,
                    OldValue = oldValue,
                    NewValue = newValue,
                    CreatedAt = DateTime.UtcNow
                };

                _context.AuditLogs.Add(auditLog);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Fallback safe logger in case of parsing error to ensure application safety
                var fallbackLog = new Audit_Log
                {
                    UserId = userId == 0 ? (int?)null : userId,
                    Action = "OTHER",
                    TableName = "System",
                    NewValue = $"Fallback log due to error: {ex.Message}. Raw event: " + JsonSerializer.Serialize(eventObj),
                    CreatedAt = DateTime.UtcNow
                };
                _context.AuditLogs.Add(fallbackLog);
                await _context.SaveChangesAsync();
            }
        }
    }
}
