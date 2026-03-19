using Microsoft.AspNetCore.Mvc;
using QUANTRIKHACHSANN5.DTOs.Booking;
using QUANTRIKHACHSANN5.Interfaces;

namespace QUANTRIKHACHSANN5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> BookRoom([FromBody] CreateBookingDTO dto)
        {
            var result = await _bookingService.CreateBookingAsync(dto);
            if (result)
                return Ok(new { Message = "Đặt phòng thành công!" });
            
            return BadRequest(new { Message = "Đặt phòng thất bại. Vui lòng kiểm tra lại." });
        }
    }
}