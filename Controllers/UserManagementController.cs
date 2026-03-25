using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserManagement
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context
                .Users.Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Select(u => new
                {
                    id = u.Id,
                    fullName = u.FullName,
                    email = u.Email,
                    roleName = u.UserRoles.FirstOrDefault() != null
                        ? u.UserRoles.FirstOrDefault().Role.Name
                        : "Chưa cấp quyền",
                    isActive = true, // Tạm thời cho True (Hoặc u.IsActive nếu DB ní có cột này)
                })
                .ToListAsync();

            return Ok(users);
        }

        // POST: api/UserManagement
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var exist = await _context.Users.AnyAsync(u => u.Email == request.Email);
            if (exist)
                return BadRequest(new { message = "Email này đã được sử dụng!" });

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            if (request.RoleId > 0)
            {
                _context.UserRoles.Add(new User_Role { UserId = user.Id, RoleId = request.RoleId });
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Tạo tài khoản thành công! Mật khẩu mặc định là 123456" });
        }

        // ĐÃ ĐƯA HÀM NÀY VÀO BÊN TRONG CLASS CONTROLLER
        // PUT: api/UserManagement/{id}
        // PUT: api/UserManagement/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            var user = await _context
                .Users.Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound(new { message = "Không tìm thấy người dùng!" });

            // 1. Cập nhật thông tin cơ bản
            user.FullName = request.FullName;
            // user.PhoneNumber = request.PhoneNumber;

            // 2. Cập nhật chức vụ (Role) theo đúng luật EF Core: Xóa cũ -> Thêm mới
            var currentRole = user.UserRoles.FirstOrDefault();

            // Bước A: Nếu có role cũ thì xóa nó đi
            if (currentRole != null)
            {
                _context.UserRoles.Remove(currentRole);
            }

            // Bước B: Thêm role mới vào
            if (request.RoleId > 0)
            {
                _context.UserRoles.Add(new User_Role { UserId = user.Id, RoleId = request.RoleId });
            }

            // 3. Lưu lại vào Database
            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật thành công!" });
        }

        // ĐÃ ĐƯA HÀM NÀY VÀO BÊN TRONG CLASS CONTROLLER
        // PUT: api/UserManagement/{id}/toggle-status
        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "Không tìm thấy người dùng!" });

            user.IsActive = !user.IsActive;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật trạng thái thành công!" });
        }
    }

    // Các class phụ trợ nằm ngoài Controller nhưng vẫn trong Namespace
    public class CreateUserRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
    }

    public class UpdateUserRequest
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public int RoleId { get; set; }
    }
}
