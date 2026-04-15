using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuditLogsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AuditLogsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetAuditLogs(
        [FromQuery] int? userId = null,
        [FromQuery] string? action = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = _context.AuditLogs
            .Include(al => al.User)
            .AsQueryable();

        if (userId.HasValue)
            query = query.Where(al => al.UserId == userId.Value);
        
        if (!string.IsNullOrEmpty(action))
            query = query.Where(al => al.Action == action);
        
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
                Action = al.Action,
                TargetTable = al.TargetTable,
                Status = al.Status,
                EventId = al.EventId,
                Timestamp = al.Timestamp,
                LogData = al.LogData
            })
            .ToListAsync();

        return Ok(new { Data = logs, Total = total, Page = page, PageSize = pageSize });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuditLogDto>> GetAuditLog(int id)
    {
        var auditLog = await _context.AuditLogs
            .Include(al => al.User)
            .FirstOrDefaultAsync(al => al.Id == id);

        if (auditLog == null)
            return NotFound();

        var dto = new AuditLogDto
        {
            Id = auditLog.Id,
            UserId = auditLog.UserId,
            UserName = auditLog.User != null ? auditLog.User.FullName : null,
            RoleName = auditLog.RoleName,
            Action = auditLog.Action,
            TargetTable = auditLog.TargetTable,
            Status = auditLog.Status,
            EventId = auditLog.EventId,
            Timestamp = auditLog.Timestamp,
            LogData = auditLog.LogData
        };

        return Ok(dto);
    }
}

public class AuditLogDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string TargetTable { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string EventId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string? LogData { get; set; }
}
