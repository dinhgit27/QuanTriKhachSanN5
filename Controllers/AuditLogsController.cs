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
                UserName = al.User != null ? al.User.FullName : null,
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
            UserName = auditLog.User != null ? auditLog.User.FullName : null,
            RoleName = auditLog.RoleName,
            Timestamp = auditLog.Timestamp,
            LogData = auditLog.LogData,
        };

        return Ok(dto);
    }

    /// <summary>
    /// Create batch audit log - accepts {TotalEvents, Events: [{eventId, actionType, targetTable, ...}]}
    /// Saves as single Audit_Log with minified JSON in LogData
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> CreateBatchAuditLog([FromBody] object payload)
    {
        try
        {
            // Get current user info
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var roleName = User.FindFirst(ClaimTypes.Role)?.Value ?? "Unknown";

            // Minify JSON
            var options = new JsonSerializerOptions { WriteIndented = false };
            var jsonString = JsonSerializer.Serialize(payload, options);

            var auditLog = new Audit_Log
            {
                UserId = userId,
                RoleName = roleName,
                Timestamp = DateTime.UtcNow,
                LogData = jsonString,
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

    // NOTE: FlushBatch removed because it's no longer needed with immediate database persistence.
}

public class AuditLogDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string? LogData { get; set; }
}
