using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Interfaces;
using System.Threading.Tasks;
using System.Linq;

namespace QuanTriKhachSanN5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceptionController : ControllerBase
    {
        private readonly IReceptionService _receptionService;
        private readonly ApplicationDbContext _context;

        public ReceptionController(IReceptionService receptionService, ApplicationDbContext context)
        {
            _receptionService = receptionService;
            _context = context;
        }

        // 1. NÚT CHECK-IN
        [HttpPost("checkin/{bookingId}")]
        public async Task<IActionResult> CheckIn(int bookingId, [FromBody] int roomId)
        {
            await _receptionService.CheckInBookingAsync(bookingId, roomId);
            return Ok(new { message = "Check-in và giao phòng thành công!" });
        }

        // 2. NÚT THÊM DỊCH VỤ
        [HttpPost("order-service/{bookingId}")]
        public async Task<IActionResult> OrderService(int bookingId, [FromBody] ServiceRequest req)
        {
            try 
            {
                await _receptionService.OrderServiceAsync(bookingId, req.ServiceId, req.Quantity);
                return Ok(new { message = "Thêm dịch vụ thành công!" });
            }
            catch (Exception ex)
            {
                // 🚨 BẮT TẬN TAY LỖI SQL VÀ NÉM LÊN CHO REACT
                string errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest(new { message = "Lỗi Database: " + errorMsg });
            }
        }
        // LẤY DANH SÁCH DỊCH VỤ CHO DROPDOWN
        [HttpGet("services-list")]
        public async Task<IActionResult> GetServicesList()
        {
            // Lấy toàn bộ dịch vụ trong database (Tên và Giá)
            var services = await _context.Services
                .Select(s => new { 
                    id = s.Id, 
                    name = s.Name, // Chú ý: Nếu Model của ní dùng chữ ServiceName thì đổi lại nha
                    price = s.Price 
                })
                .ToListAsync();
            return Ok(services);
        }

        // 3. LẤY PHÒNG TRỐNG CHO MODAL CHECK-IN
        [HttpGet("available-rooms")]
        public async Task<IActionResult> GetAvailableRooms()
        {
            // Lấy các phòng đang trống, sạch sẽ để Lễ tân chọn giao cho khách
            var rooms = await _context.Rooms
                .Where(r => r.Status == "Available" || r.Status == "Trống")
                .Select(r => new { r.Id, r.RoomNumber })
                .ToListAsync();
            return Ok(rooms);
        }
    }

    public class ServiceRequest
    {
        public int ServiceId { get; set; }
        public int Quantity { get; set; }
    }
}