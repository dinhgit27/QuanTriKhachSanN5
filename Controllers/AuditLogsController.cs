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
        [FromQuery] int pageSize = 20
    )
    {
        var query = _context.AuditLogs.Include(al => al.User).AsQueryable();

        if (userId.HasValue)
            query = query.Where(al => al.UserId == userId.Value);

        if (fromDate.HasValue)
            query = query.Where(al => al.Timestamp >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(al => al.Timestamp <= toDate.Value);

        var total = await query.CountAsync();
        var logs = await query
            .OrderByDescending(al => al.Timestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(al => new AuditLogDto
            {
                Id = al.Id,
                UserId = al.UserId,
                UserName = al.User != null ? al.User.FullName : (al.UserId == null ? "Hệ thống" : "Unknown"),
                RoleName = al.RoleName,
                Timestamp = al.Timestamp,
                LogData = al.LogData,
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
            UserName = auditLog.User != null ? auditLog.User.FullName : (auditLog.UserId == null ? "Hệ thống" : "Unknown"),
            RoleName = auditLog.RoleName,
            Timestamp = auditLog.Timestamp,
            LogData = auditLog.LogData,
        };

        return Ok(dto);
    }

    /// <summary>
    /// Create batch audit log - accepts {TotalEvents, Events: [{eventId, actionType, targetTable, ...}]}
    /// Saves using AuditBatchService to handle daily merging
    /// </summary>
[HttpPost]
    public async Task<ActionResult> CreateBatchAuditLog([FromBody] AuditLogPayloadRequest payload)
    {
        try
        {
            if (payload == null)
                return BadRequest(new { message = "Payload is null" });
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userIdClaim, out int userId);
            var roleName = User.FindFirst(ClaimTypes.Role)?.Value ?? "Hệ thống";

            if (payload.Events == null || !payload.Events.Any())
                return BadRequest("No events provided.");

            var processedEvents = payload.Events.Select(e => new {
                eventId = e.GetProperty("eventId").GetString(),
                actionType = e.GetProperty("actionType").GetString(),
                description = e.TryGetProperty("description", out var desc) ? desc.GetString() : "",
                userName = e.TryGetProperty("userName", out var user) ? user.GetString() : "System",
                timestamp = e.GetProperty("timestamp").GetDateTime()
            }).ToList();

            foreach (var evt in processedEvents)
            {
                await _batchService.AddEventAsync(userId > 0 ? userId : null, roleName, evt);
            }

            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            // Tránh gây "empty response" ở frontend: luôn trả JSON lỗi rõ ràng
            return StatusCode(500, new { message = $"Error saving batch log: {ex.Message}", detail = ex.ToString() });
        }
    }

    // NOTE: FlushBatch removed because it's no longer needed with immediate database persistence.
}

public class AuditLogDto
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string? UserName { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string? LogData { get; set; }
}

public class AuditLogPayloadRequest
{
    public int TotalEvents { get; set; }
    public List<JsonElement> Events { get; set; } = new List<JsonElement>();
}
