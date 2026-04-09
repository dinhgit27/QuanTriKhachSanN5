using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.DTOs;
using QuanTriKhachSanN5.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace QuanTriKhachSanN5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase 
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("available-rooms")]
        public async Task<IActionResult> GetAvailableRooms([FromBody] CheckAvailableRequest req)
        {
            if (req.CheckIn >= req.CheckOut)
                return BadRequest(new { message = "Ngày Check-out phải lớn hơn Check-in!" });

            var bookedRoomIds = await _context.BookingDetails
                .Where(bd => bd.CheckInDate < req.CheckOut 
                          && bd.CheckOutDate > req.CheckIn
                          && bd.Booking.Status != "Cancelled")
                .Where(bd => bd.RoomId != null)
                .Select(bd => bd.RoomId.Value)
                .ToListAsync();

            var availableRoomTypes = await _context.RoomTypes
                .Include(rt => rt.Rooms.Where(r => 
                    !bookedRoomIds.Contains(r.Id) && 
                    r.Status != "Maintenance" 
                    // Tui đã xóa r.IsActive ở đây để không bị lỗi nữa!
                ))
                .Where(rt => rt.CapacityAdults >= req.Adults) 
                .ToListAsync();

            var result = availableRoomTypes
                .Where(rt => rt.Rooms.Any())
                .Select(rt => new
                {
                    RoomTypeId = rt.Id,
                    RoomTypeName = rt.Name,
                    PricePerNight = rt.BasePrice,
                    CapacityAdults = rt.CapacityAdults,
                    CapacityChildren = rt.CapacityChildren,
                    AvailableRooms = rt.Rooms.Select(r => new { r.Id, r.RoomNumber, r.Floor }).ToList()
                });

            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest req)
        {
            if (!req.SelectedRoomIds.Any())
                return BadRequest(new { message = "Vui lòng chọn ít nhất 1 phòng!" });

            string bookingCode = $"BK-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000,9999)}";

            var newBooking = new Booking
            {
                GuestName = req.GuestName,
                GuestPhone = req.GuestPhone,
                GuestEmail = req.GuestEmail,
                BookingCode = bookingCode,
                Status = "Confirmed",
                BookingDetails = new List<BookingDetail>() 
            };

            foreach (var roomId in req.SelectedRoomIds)
            {
                var room = await _context.Rooms.Include(r => r.RoomType).FirstOrDefaultAsync(r => r.Id == roomId);
                if (room != null && room.RoomType != null)
                {
                    newBooking.BookingDetails.Add(new BookingDetail 
                    {
                        RoomId = room.Id,
                        RoomTypeId = room.RoomTypeId,
                        CheckInDate = req.CheckIn,
                        CheckOutDate = req.CheckOut,
                        // 🚨 CHỖ NÀY ĐÂY: Xóa bỏ ?? 0m vì BasePrice vốn đã không null
                        PricePerNight = room.RoomType.BasePrice 
                    });
                }
            }

            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đặt phòng thành công!", bookingCode = newBooking.BookingCode });
        }
    }
}