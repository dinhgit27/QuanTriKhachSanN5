using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderServicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL ORDER
        // =========================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderService>>> GetAll()
        {
            return await _context.OrderServices.ToListAsync();
        }

        // =========================
        // GET BY ID
        // =========================
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderService>> GetById(int id)
        {
            var order = await _context.OrderServices.FindAsync(id);

            if (order == null)
                return NotFound();

            return order;
        }

        // =========================
        // KHÁCH GỌI DỊCH VỤ
        // =========================
        [HttpPost("order")]
public IActionResult CreateOrder([FromBody] OrderRequest request)
{
    // 1. tạo order
    var order = new OrderService
    {
        booking_detail_id = request.BookingDetailId,
        total_amount = 0,
        
    };

    _context.Order_Services.Add(order);
    _context.SaveChanges();

    decimal total = 0;

    // 2. lưu từng món
    foreach (var item in request.Items)
    {
        var service = _context.Services.Find(item.ServiceId);

        if (service == null)
            return BadRequest("Service không tồn tại");

        var detail = new OrderServiceDetail
        {
            order_service_id = order.id,
            service_id = item.ServiceId,
            quantity = item.Quantity,
            unit_price = service.price
        };

        total += item.Quantity * service.price;

        _context.Order_Service_Details.Add(detail);
    }

    // 3. update total
    order.total_amount = total;
    _context.SaveChanges();

    return Ok(order);
}

        // =========================
        // CẬP NHẬT TRẠNG THÁI
        // =========================
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _context.OrderServices.FindAsync(id);

            if (order == null)
                return NotFound();

            order.status = status;

            await _context.SaveChangesAsync();

            return Ok("Cập nhật trạng thái thành công");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var order = await _context.Order_Services.FindAsync(id);

            if (order == null)
            return NotFound();

            order.status = "Cancelled";

            await _context.SaveChangesAsync();

            return Ok(order);
        }
    }
}