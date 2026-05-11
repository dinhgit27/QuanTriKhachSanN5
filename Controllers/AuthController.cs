using BCrypt.Net;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.DTOs;
using QuanTriKhachSanN5.Models;
using QuanTriKhachSanN5.Services;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;

namespace QuanTriKhachSanN5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwt;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;

        public AuthController(ApplicationDbContext context, JwtService jwt, IMemoryCache cache, IEmailService emailService)
        {
            _context = context;
            _jwt = jwt;
            _cache = cache;
            _emailService = emailService;
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Email))
                return BadRequest(new { message = "Email không được để trống!" });

            string email = dto.Email.Trim().ToLower();
            
            // Nếu là tài khoản test (@hotel.com), bỏ qua cấp OTP ngẫu nhiên, set cứng OTP là 123456
            if (email.EndsWith("@hotel.com"))
            {
                _cache.Set(email, "123456", TimeSpan.FromMinutes(5));
                return Ok(new { message = "Đã gửi mã OTP (Tài khoản Test: Mặc định là 123456)" });
            }

            // Nếu không phải @gmail.com thì chặn
            if (!email.EndsWith("@gmail.com"))
            {
                return BadRequest(new { message = "Chỉ chấp nhận đăng ký bằng tài khoản @gmail.com hoặc email test!" });
            }

            // Sinh mã OTP 6 số ngẫu nhiên
            var random = new Random();
            string otp = random.Next(100000, 999999).ToString();

            // Lưu vào Cache 5 phút
            _cache.Set(email, otp, TimeSpan.FromMinutes(5));

            // Gọi hàm gửi email
            string subject = "Mã xác nhận đăng ký tài khoản IT HOTEL";
            string body = $@"
                <h3>Chào bạn,</h3>
                <p>Cảm ơn bạn đã đăng ký tài khoản tại hệ thống IT HOTEL.</p>
                <p>Mã xác thực OTP của bạn là: <b style='font-size: 20px; color: #e6b83b;'>{otp}</b></p>
                <p>Mã này có hiệu lực trong vòng 5 phút.</p>
                <p>Trân trọng,<br>IT HOTEL Team</p>
            ";

            try
            {
                await _emailService.SendEmailAsync(email, subject, body);
                return Ok(new { message = "Mã OTP đã được gửi đến email của bạn!" });
            }
            catch (Exception ex)
            {
                // Nếu lỗi SMTP (do chưa điền mật khẩu thật), báo lỗi chi tiết
                return StatusCode(500, new { message = "Lỗi khi gửi email: " + ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Otp))
                return BadRequest(new { message = "Vui lòng nhập mã OTP!" });

            string email = dto.Email.Trim().ToLower();

            // 1. Kiểm tra OTP
            if (!_cache.TryGetValue(email, out string savedOtp))
            {
                return BadRequest(new { message = "Mã OTP đã hết hạn hoặc không tồn tại. Vui lòng lấy mã mới!" });
            }

            if (dto.Otp != savedOtp)
            {
                return BadRequest(new { message = "Mã OTP không chính xác!" });
            }

            // 2. Xóa OTP sau khi dùng thành công
            _cache.Remove(email);

            var exist = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (exist != null)
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
            if (!user.IsActive)
            {
                return Unauthorized(new { message = "Tài khoản của bạn đã bị khóa!" });
            }

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

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Không tìm thấy thông tin user!" });

            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound(new { message = "User không tồn tại!" });

            var membership = await _context.Memberships
                .Where(m => m.Id == user.MembershipId)
                .FirstOrDefaultAsync();

            return Ok(new
            {
                id = user.Id,
                email = user.Email,
                fullName = user.FullName,
                points = user.Points,
                membership = membership != null ? new
                {
                    id = membership.Id,
                    tierName = membership.Level, // Giả sử Level là tier_name
                    minPoints = membership.Points, // Giả sử Points là min_points
                    discountPercent = membership.DiscountPercent
                } : null,
                roleName = user.UserRoles.FirstOrDefault()?.Role.Name
            });
        }
    }
}
