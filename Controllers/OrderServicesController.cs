using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin,Receptionist")] // Chỉ Lễ tân và Quản lý mới được xem toàn bộ danh sách đơn
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderService>>> GetAll()
        {
            return await _context.OrderServices.ToListAsync();
        }

        // =========================
        // GET BY ID
        // =========================
        [Authorize] // Bất kỳ ai đăng nhập (Khách, Lễ tân...) đều có thể xem chi tiết đơn
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
        [Authorize(Roles = "Guest,Receptionist,Admin")] // Khách hàng tự đặt hoặc Lễ tân đặt hộ
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


    decimal total = 0;
    var details = new List<OrderServiceDetail>();

    // 2. lưu từng món
    if (request.Items != null)
    {
        foreach (var item in request.Items)
        {
            var service = _context.Services.Find(item.ServiceId);

            if (service == null)
                return BadRequest($"Service với ID {item.ServiceId} không tồn tại");

            details.Add(new OrderServiceDetail
            {
                ServiceId = item.ServiceId,
                Quantity = item.Quantity,
                UnitPrice = service.Price
            });

            total += item.Quantity * service.Price;
        }
    }

    // 3. update total
    order.TotalAmount = total;
    order.Details = details; // EF Core sẽ tự động nối OrderServiceId
    _context.OrderServices.Add(order);
    _context.SaveChanges();

    return Ok(order);
}

        // =========================
        // CẬP NHẬT TRẠNG THÁI
        // =========================
        [Authorize(Roles = "Admin,Receptionist")] // Chỉ nhân viên mới được phép duyệt/cập nhật trạng thái
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
        
        [Authorize(Roles = "Admin,Receptionist")] // Chỉ nhân viên mới được hủy đơn
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