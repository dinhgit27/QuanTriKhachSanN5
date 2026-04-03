using BCrypt.Net;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.DTOs;
using QuanTriKhachSanN5.Models;
using QuanTriKhachSanN5.Services;

namespace QuanTriKhachSanN5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwt;

        public AuthController(ApplicationDbContext context, JwtService jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var exist = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (exist != null)
                // Trả về dạng JSON (new { message = ... }) để Frontend React dễ bắt lỗi
                return BadRequest(new { message = "Email đã tồn tại!" });

            var user = new User
            {
                FullName = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Gán role Guest theo RBAC
            var guestRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Guest");
            if (guestRole != null)
            {
                _context.UserRoles.Add(new User_Role { UserId = user.Id, RoleId = guestRole.Id });
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Đăng ký thành công!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var user = await _context
                .Users.Include(u => u.UserRoles)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            // 1. TRẠM KIỂM SOÁT 1: Bắt buộc phải kiểm tra null ĐẦU TIÊN
            if (user == null)
                return Unauthorized(new { message = "Tài khoản hoặc mật khẩu không đúng!" });

            // 2. TRẠM KIỂM SOÁT 2: Kiểm tra mật khẩu
            bool check = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!check)
                return Unauthorized(new { message = "Tài khoản hoặc mật khẩu không đúng!" });

            // (Tùy chọn) TRẠM KIỂM SOÁT 3: Kiểm tra trạng thái tài khoản (Soft Delete)
            // if (user.IsActive == false)
            //     return Unauthorized(new { message = "Tài khoản của bạn đã bị khóa!" });

            // 3. Vượt qua hết mới bắt đầu lấy dữ liệu Role và Permission
            var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();

            var permissions = user
                .UserRoles.SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission.Name)
                .Distinct()
                .ToList();

            // 4. Sinh Token
            var token = _jwt.GenerateToken(user, roles, permissions);

            // 5. Trả về đúng cấu trúc mà React Frontend đang chờ để lưu vào Zustand store
            return Ok(
                new
                {
                    token = token,
                    user = new
                    {
                        id = user.Id,
                        email = user.Email,
                        fullName = user.FullName,
                    },
                    permissions = permissions,
                }
            );
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginDTO dto)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == payload.Email);

            if (user == null)
            {
                user = new User { Email = payload.Email, FullName = payload.Name };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Gán role Guest
                var guestRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Guest");
                if (guestRole != null)
                {
                    _context.UserRoles.Add(
                        new User_Role { UserId = user.Id, RoleId = guestRole.Id }
                    );
                    await _context.SaveChangesAsync();
                }
            }

            var roles = await _context
                .UserRoles.Where(ur => ur.UserId == user.Id)
                .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                .ToListAsync();

            var permissions = await _context
                .UserRoles.Where(ur => ur.UserId == user.Id)
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission.Name)
                .Distinct()
                .ToListAsync();

            var token = _jwt.GenerateToken(user, roles, permissions);

            // Đồng bộ Response trả về giống hàm Login thường
            return Ok(
                new
                {
                    token = token,
                    user = new
                    {
                        id = user.Id,
                        email = user.Email,
                        fullName = user.FullName,
                        roleName = user.UserRoles.FirstOrDefault()?.Role.Name,
                    },
                    permissions = permissions,
                }
            );
        }
    }
}
