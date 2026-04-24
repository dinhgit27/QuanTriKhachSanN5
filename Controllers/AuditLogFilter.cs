using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

using QuanTriKhachSanN5.Interfaces;

namespace QuanTriKhachSanN5.Filters
{
    public class AuditLogFilter : IAsyncActionFilter
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditBatchService _batchService;

        public AuditLogFilter(ApplicationDbContext context, IAuditBatchService batchService)
        {
            _context = context;
            _batchService = batchService;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next
        )
        {
            // 1. Trích xuất thông tin HTTP Request
            var method = context.HttpContext.Request.Method;
            var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            // 🚨 QUAN TRỌNG: Nếu không có UserId (chưa đăng nhập), KHÔNG LƯU DATABASE để tránh rác "Hệ thống"
            if (string.IsNullOrEmpty(userIdClaim))
            {
                await next();
                return;
            }

            int.TryParse(userIdClaim, out int userId);

            // 2. Cho phép API (Controller) thực thi nghiệp vụ chính
            var resultContext = await next();

            // 3. Nếu API chạy THÀNH CÔNG và LÀ THAO TÁC THAY ĐỔI DỮ LIỆU (CUD)
            if (
                resultContext.Exception == null
                && resultContext.HttpContext.Response.StatusCode >= 200 
                && resultContext.HttpContext.Response.StatusCode < 300
                && (method == "POST" || method == "PUT" || method == "DELETE")
            )
            {
                var roleName = context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value ?? "Hệ thống";
                var fullName = context.HttpContext.User.FindFirst("FullName")?.Value ?? "Hệ thống";
                var controllerName = context.RouteData.Values["controller"]?.ToString() ?? "Unknown";
                
                // Tránh việc log chính các yêu cầu lưu log
                if (controllerName == "AuditLogs") return;

                var actionType = method == "POST" ? "CREATE" : (method == "PUT" ? "UPDATE" : "DELETE");
                var eventId = Guid.NewGuid().ToString("N").Substring(0, 8);
                
                var eventObj = new {
                    eventId = eventId,
                    actionType = actionType,
                    targetTable = controllerName,
                    userName = fullName,
                    status = "Success",
                    timestamp = DateTime.UtcNow,
                    description = $"Thực hiện {actionType} trên {controllerName}"
                };

                await _batchService.AddEventAsync(userId > 0 ? userId : null, roleName, eventObj);
            }
        }
    }
}
