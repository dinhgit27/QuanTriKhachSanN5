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
    // 0. Lấy thông tin BookingId từ BookingDetail
    var bookingDetail = _context.BookingDetails.Find(request.BookingDetailId);
    
    if (bookingDetail == null)
        return BadRequest("Chi tiết đặt phòng (BookingDetail) không tồn tại.");

    // 1. tạo order
    var order = new OrderService
    {
        BookingDetailId = request.BookingDetailId,
        BookingId = bookingDetail.BookingId, // Bắt buộc phải có khóa ngoại này
        OrderDate = DateTime.Now,
        Status = "Pending",
        TotalAmount = 0
    };

    _context.OrderServices.Add(order);
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
            OrderServiceId = order.Id,
            ServiceId = item.ServiceId,
            Quantity = item.Quantity,
            UnitPrice = service.Price
        };

        total += item.Quantity * service.Price;

        _context.OrderServiceDetails.Add(detail);
    }

    // 3. update total
    order.TotalAmount = total;
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

            order.Status = status;

            await _context.SaveChangesAsync();

            return Ok("Cập nhật trạng thái thành công");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var order = await _context.OrderServices.FindAsync(id);

            if (order == null)
            return NotFound();

            order.Status = "Cancelled";

            await _context.SaveChangesAsync();

            return Ok(order);
        }
    }
}