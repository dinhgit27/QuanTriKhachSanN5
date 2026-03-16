using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public ServicesController(ApplicationDbContext context)

        {
            _context = context;
        }

        // ==============================
        // LẤY DANH SÁCH SERVICES
        // ==============================
        // GET: api/services
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices()
    {
        return await _context.Services
        .Where(s => s.status == 1)
        .ToListAsync();
    }

        // ==============================
        // LẤY SERVICE THEO ID
        // ==============================
        // GET: api/services/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetService(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return service;
        }

        // ==============================
        // THÊM SERVICE
        // ==============================
        // POST: api/services
        [HttpPost]
        public async Task<ActionResult<Service>> PostService(Service service)
        {
            // thêm dịch vụ vào database
            _context.Services.Add(service);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetService), new { id = service.id }, service);
        }

        // ==============================
        // SỬA SERVICE
        // ==============================
        // PUT: api/services/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutService(int id, Service service)
        {
            if (id != service.id)
            {
                return BadRequest();
            }

            // cập nhật dữ liệu
            _context.Entry(service).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ==============================
        // DISABLE SERVICE (KHÔNG XOÁ)
        // ==============================
        // DELETE: api/services/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DisableService(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            // Không xoá dữ liệu
            // Chỉ chuyển trạng thái sang disable
            service.status = 0;

            await _context.SaveChangesAsync();

            return Ok("Service đã được disable");
        }

        // ==============================
        // ENABLE SERVICE
        // ==============================
        // PUT: api/services/enable/5
        [HttpPut("enable/{id}")]
        public async Task<IActionResult> EnableService(int id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            // bật lại dịch vụ
            service.status = 1;

            await _context.SaveChangesAsync();

            return Ok("Service đã được enable");
        }
    }
}