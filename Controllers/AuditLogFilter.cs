using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Models;

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
            var userIdClaim = context.HttpContext.User.FindFirst("UserId")?.Value;
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
                    var controllerName =
                        context.RouteData.Values["controller"]?.ToString() ?? "Unknown";
                    var recordIdStr = context.RouteData.Values["id"]?.ToString();
                    int.TryParse(recordIdStr, out int recordId);

                    var auditLog = new Audit_Log
                    {
                        UserId = userId,
                        Action = method, // POST (Tạo), PUT (Sửa), DELETE (Xóa)
                        TableName = controllerName, // Lấy tên Controller làm đại diện cho Entity
                        RecordId = recordId,
                        Timestamp = DateTime.UtcNow,
                        Details = $"Thực thi API {method} thành công trên {controllerName}",
                    };

                    _context.AuditLogs.Add(auditLog);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
