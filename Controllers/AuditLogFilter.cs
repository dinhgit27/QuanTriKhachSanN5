using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace QuanTriKhachSanN5.Filters
{
    public class AuditLogFilter : IAsyncActionFilter
    {
        private readonly ApplicationDbContext _context;

        public AuditLogFilter(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next
        )
        {
            // 1. Trích xuất thông tin HTTP Request
            var method = context.HttpContext.Request.Method;
            var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userIdClaim, out int userId);

            // 2. Cho phép API (Controller) thực thi nghiệp vụ chính
            var resultContext = await next();

            // 3. Nếu API chạy THÀNH CÔNG và LÀ THAO TÁC THAY ĐỔI DỮ LIỆU (CUD)
            if (
                resultContext.Exception == null
                && (method == "POST" || method == "PUT" || method == "DELETE")
            )
            {
                if (userId > 0) // Chỉ log nếu thao tác do user đã đăng nhập thực hiện
                {
                    var roleName = context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value ?? "Unknown";
                    var controllerName = context.RouteData.Values["controller"]?.ToString() ?? "Unknown";

                    var actionType = method == "POST" ? "CREATE" : (method == "PUT" ? "UPDATE" : "DELETE");
                    var actionDetailObj = new {
                        eventId = Guid.NewGuid().ToString("N").Substring(0, 8),
                        actionType = actionType,
                        targetTable = controllerName,
                        status = "Success",
                        timestamp = DateTime.UtcNow
                    };
                    var auditLog = new Audit_Log
                    {
                        UserId = userId,
                        RoleName = roleName,
                        Action = actionType,
                        TargetTable = controllerName,
                        Status = "Success",
                        EventId = Guid.NewGuid().ToString("N").Substring(0, 8),
                        Timestamp = DateTime.UtcNow,
                        LogData = actionDetailJson
                    };
                    
                    _context.AuditLogs.Add(auditLog);
                    try 
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"AuditLog save error: {ex.Message}");
                    }
                }
            }
        }
    }
}
