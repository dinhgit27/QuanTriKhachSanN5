using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;

namespace QuanTriKhachSanN5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            // Lấy danh sách ID và Tên của các chức vụ để hiển thị lên Dropdown (Select)
            var roles = await _context.Roles
                .Select(r => new { id = r.Id, name = r.Name })
                .ToListAsync();
            
            return Ok(roles);
        }
    }
}