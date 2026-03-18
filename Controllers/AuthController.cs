using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.DTOs;
using QuanTriKhachSanN5.Models;
using QuanTriKhachSanN5.Services;
using Google.Apis.Auth;

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
            var exist = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (exist != null)
                return BadRequest("Email đã tồn tại!");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Customer"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Đăng ký thành công!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null)
                return Unauthorized("Email không đúng!");

            bool check = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!check)
                return Unauthorized("Sai mật khẩu!");

            var token = _jwt.GenerateToken(user);

            return Ok(new { token });
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginDTO dto)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken);

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == payload.Email);

            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email,
                    Username = payload.Name,
                    Role = "Customer"
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            var token = _jwt.GenerateToken(user);

            return Ok(new { token });
        }
    }
}