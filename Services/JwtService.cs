using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user, List<string> roles, List<string> permissions)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Email, user.Email) };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            claims.AddRange(permissions.Select(p => new Claim("permission", p)));

            foreach (var perm in permissions)
            {
                claims.Add(new Claim("Permission", perm));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
