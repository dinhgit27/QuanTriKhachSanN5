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
                return BadRequest("Email đã tồn tại!");

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

            return Ok("Đăng ký thành công!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var user = await _context
                .Users.Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();

            if (user == null)
                return Unauthorized("Email không đúng!");

            bool check = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!check)
                return Unauthorized("Sai mật khẩu!");

            // TÍNH NĂNG NÂNG CAO: Lấy danh sách Permission từ CSDL của Role người dùng đang giữ
            var permissions = user
                .UserRoles.SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission.Name)
                .Distinct()
                .ToList();

            var token = _jwt.GenerateToken(user, roles, permissions);

            return Ok(new { token });
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

            return Ok(new { token });
        }
    }
}
