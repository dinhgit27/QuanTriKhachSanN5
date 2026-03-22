using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanTriKhachSanN5.DTOs.Room;
using QuanTriKhachSanN5.Interfaces;

namespace QuanTriKhachSanN5.Controllers.Disabled
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

        // POST: api/bookings/search-room
        // Dùng POST để gửi body request tìm kiếm cho an toàn (thay vì truyền ngày tháng lên URL)
        [HttpPost("search-room")]
        public async Task<IActionResult> SearchRoom([FromBody] SearchRoomRequestDTO request)
        {
            var result = await _bookingService.SearchAvailableRoomTypesAsync(request);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // GET: api/bookings/available-physical-rooms?roomTypeId=1&checkIn=2026-03-10&checkOut=2026-03-12
        [HttpGet("available-physical-rooms")]
        public async Task<IActionResult> GetAvailablePhysicalRooms(
            [FromQuery] int roomTypeId,
            [FromQuery] DateTime checkIn,
            [FromQuery] DateTime checkOut
        )
        {
            var result = await _bookingService.GetAvailablePhysicalRoomsAsync(
                roomTypeId,
                checkIn,
                checkOut
            );
            return Ok(result);
        }
    }
}
