using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuditLogsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IAuditBatchService _batchService;

    public AuditLogsController(ApplicationDbContext context, IAuditBatchService batchService)
    {
        _context = context;
        _batchService = batchService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetAuditLogs(
        [FromQuery] int? userId = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 500
    )
    {
        var query = _context.AuditLogs.Include(al => al.User).AsQueryable();

        if (userId.HasValue)
            query = query.Where(al => al.UserId == userId.Value);

        if (fromDate.HasValue)
            query = query.Where(al => al.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(al => al.CreatedAt <= toDate.Value);

        var total = await query.CountAsync();
        var logs = await query
            .OrderByDescending(al => al.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(al => new AuditLogDto
            {
                Id = al.Id,
                UserId = al.UserId,
                UserName = al.User != null ? al.User.FullName : null,
                Action = al.Action,
                TableName = al.TableName,
                RecordId = al.RecordId,
                OldValue = al.OldValue,
                NewValue = al.NewValue,
                CreatedAt = al.CreatedAt
            })
            .ToListAsync();

        return Ok(
            new
            {
                Data = logs,
                Total = total,
                Page = page,
                PageSize = pageSize,
            }
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuditLogDto>> GetAuditLog(int id)
    {
        var auditLog = await _context
            .AuditLogs.Include(al => al.User)
            .FirstOrDefaultAsync(al => al.Id == id);

        if (auditLog == null)
            return NotFound();

        var dto = new AuditLogDto
        {
            Id = auditLog.Id,
            UserId = auditLog.UserId,
            UserName = auditLog.User != null ? auditLog.User.FullName : null,
            Action = auditLog.Action,
            TableName = auditLog.TableName,
            RecordId = auditLog.RecordId,
            OldValue = auditLog.OldValue,
            NewValue = auditLog.NewValue,
            CreatedAt = auditLog.CreatedAt
        };

        return Ok(dto);
    }

    /// <summary>
    /// Create batch audit log - accepts event payloads
    /// Saves as single Audit_Log using direct table columns
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult> CreateBatchAuditLog([FromBody] object payload)
    {
        try
        {
            // Get current user info from JWT claim if authenticated
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int? userId = null;

            if (int.TryParse(userIdClaim, out int parsedUserId))
            {
                userId = parsedUserId;
            }
            else
            {
                // Try to extract user email or user name from the JSON payload to resolve the real user!
                try
                {
                    var options = new JsonSerializerOptions { WriteIndented = false };
                    var jsonString = JsonSerializer.Serialize(payload, options);
                    using var doc = JsonDocument.Parse(jsonString);
                    var root = doc.RootElement;

                    string? email = null;
                    string? userName = null;

                    if (root.TryGetProperty("email", out var emailProp))
                    {
                        email = emailProp.GetString();
                    }
                    else if (root.TryGetProperty("events", out var eventsProp) && eventsProp.ValueKind == JsonValueKind.Array && eventsProp.GetArrayLength() > 0)
                    {
                        var firstEvent = eventsProp[0];
                        if (firstEvent.TryGetProperty("email", out var nestedEmailProp))
                            email = nestedEmailProp.GetString();
                        if (firstEvent.TryGetProperty("userName", out var nestedUserProp))
                            userName = nestedUserProp.GetString();
                    }

                    if (!string.IsNullOrEmpty(email))
                    {
                        var matchedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                        if (matchedUser != null)
                        {
                            userId = matchedUser.Id;
                        }
                    }
                    else if (!string.IsNullOrEmpty(userName))
                    {
                        var matchedUser = await _context.Users.FirstOrDefaultAsync(u => u.FullName == userName);
                        if (matchedUser != null)
                        {
                            userId = matchedUser.Id;
                        }
                    }
                }
                catch
                {
                    // Keep userId null if unresolved
                }
            }

            // Parse properties dynamically from incoming payload
            string action = "OTHER";
            string tableName = "System";
            int? recordId = null;
            string oldValue = "{}";
            string newValue = "{}";

            if (payload != null)
            {
                var options = new JsonSerializerOptions { WriteIndented = false };
                var jsonString = JsonSerializer.Serialize(payload, options);
                using var doc = JsonDocument.Parse(jsonString);
                var root = doc.RootElement;

                // Handle single log vs batch array
                if (root.TryGetProperty("events", out var eventsProp) && eventsProp.ValueKind == JsonValueKind.Array && eventsProp.GetArrayLength() > 0)
                {
                    var firstEvent = eventsProp[0];
                    if (firstEvent.TryGetProperty("actionType", out var actProp))
                        action = actProp.GetString() ?? "OTHER";
                    else if (firstEvent.TryGetProperty("action", out var actProp2))
                        action = actProp2.GetString() ?? "OTHER";

                    if (firstEvent.TryGetProperty("module", out var modProp))
                        tableName = modProp.GetString() ?? "System";
                    else if (firstEvent.TryGetProperty("targetTable", out var tblProp))
                        tableName = tblProp.GetString() ?? "System";

                    if (firstEvent.TryGetProperty("recordId", out var recProp))
                    {
                        if (recProp.ValueKind == JsonValueKind.Number)
                            recordId = recProp.GetInt32();
                        else if (recProp.ValueKind == JsonValueKind.String && int.TryParse(recProp.GetString(), out int parsedId))
                            recordId = parsedId;
                    }

                    if (firstEvent.TryGetProperty("oldValue", out var oldProp))
                        oldValue = oldProp.ValueKind == JsonValueKind.String ? (oldProp.GetString() ?? "{}") : oldProp.ToString();
                    if (firstEvent.TryGetProperty("newValue", out var newProp))
                        newValue = newProp.ValueKind == JsonValueKind.String ? (newProp.GetString() ?? "{}") : newProp.ToString();
                    else if (firstEvent.TryGetProperty("description", out var descProp))
                        newValue = descProp.GetString() ?? "{}";
                }
                else
                {
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
            }

            var auditLog = new Audit_Log
            {
                UserId = userId,
                Action = action,
                TableName = tableName,
                RecordId = recordId,
                OldValue = oldValue,
                NewValue = newValue,
                CreatedAt = DateTime.UtcNow
            };

            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, id = auditLog.Id });
        }
        catch (Exception ex)
        {
            return BadRequest($"Error saving batch log: {ex.Message}");
        }
    }
}

public class AuditLogDto
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string? UserName { get; set; }
    public string Action { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public int? RecordId { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public DateTime CreatedAt { get; set; }
}
