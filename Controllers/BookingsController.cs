using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.DTOs;
using QuanTriKhachSanN5.Models;

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

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _context
                .Bookings.Include(b => b.BookingDetails)
                .OrderByDescending(b => b.Id)
                .Select(b => new
                {
                    id = b.Id,
                    bookingCode = b.BookingCode,
                    guestName = b.GuestName,
<<<<<<< HEAD
                    guestEmail = b.GuestEmail,
                    checkInDate = b.BookingDetails.Any() ? b.BookingDetails.Min(d => d.CheckInDate) : (DateTime?)null,
                    checkOutDate = b.BookingDetails.Any() ? b.BookingDetails.Max(d => d.CheckOutDate) : (DateTime?)null,
                    totalAmount = b.BookingDetails.Sum(d => d.PricePerNight * EF.Functions.DateDiffDay(d.CheckInDate, d.CheckOutDate)),
                    status = b.Status
                })
                .ToListAsync();
            return Ok(bookings);
        }

        [HttpPost("available-rooms")]
        public async Task<IActionResult> GetAvailableRooms([FromBody] CheckAvailableRequest req)
        {
            var bookedRoomIds = await _context
                .BookingDetails.Where(bd =>
                    bd.CheckInDate < req.CheckOut
                    && bd.CheckOutDate > req.CheckIn
                    && bd.Booking.Status != "Cancelled"
                )
                .Where(bd => bd.RoomId != null)
                .Select(bd => bd.RoomId!.Value)
                .ToListAsync();

            var availableRoomTypes = await _context
                .RoomTypes.Include(rt =>
                    rt.Rooms.Where(r => !bookedRoomIds.Contains(r.Id) && r.Status != "Maintenance")
                )
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
                    AvailableRooms = rt
                        .Rooms.Select(r => new
                        {
                            r.Id,
                            r.RoomNumber,
                            r.Floor,
                        })
                        .ToList(),
                });
            return Ok(result);
        }

        [HttpGet("arrivals")]
        public async Task<IActionResult> GetArrivalsToday()
        {
            var today = DateTime.Today;

            var arrivals = await _context.Bookings
                .Include(b => b.BookingDetails!).ThenInclude(bd => bd.RoomType!)
                .Include(b => b.BookingDetails!).ThenInclude(bd => bd.Room!) 
                // Cho phép hiển thị cả Pending và Confirmed
                .Where(b => (b.Status == "Confirmed" || b.Status == "Pending") 
                         && b.BookingDetails.Any(bd => bd.CheckInDate.Date == today))
                .Select(b => new {
                    id = b.Id,
                    bookingCode = b.BookingCode,
                    guestName = b.GuestName,
                    guestPhone = b.GuestPhone,
                    roomTypeName = b.BookingDetails.FirstOrDefault().RoomType != null
                        ? b.BookingDetails.FirstOrDefault().RoomType.Name
                        : "N/A",

                    assignedRoomId = b.BookingDetails.FirstOrDefault().RoomId,
                    assignedRoomNumber = b.BookingDetails.FirstOrDefault().Room != null
                        ? b.BookingDetails.FirstOrDefault().Room.RoomNumber
                        : null,

                    checkInDate = b.BookingDetails.Min(d => d.CheckInDate),
                    checkOutDate = b.BookingDetails.Max(d => d.CheckOutDate),
                    status = b.Status,
                })
                .ToListAsync();

            return Ok(arrivals);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest req)
        {
            if (!req.SelectedRoomIds.Any())
                return BadRequest(new { message = "Vui lòng chọn ít nhất 1 phòng!" });

            // 🚨 KIỂM TRA OVERBOOKING: Kiểm tra xem các phòng này đã có ai đặt chưa trong tầm ngày này
            var overlappingBookings = await _context.BookingDetails
                .Where(bd => req.SelectedRoomIds.Contains(bd.RoomId ?? 0))
                .Where(bd => bd.CheckInDate < req.CheckOut && bd.CheckOutDate > req.CheckIn && bd.Booking.Status != "Cancelled")
                .AnyAsync();

            if (overlappingBookings)
                return BadRequest(new { message = "Một hoặc nhiều phòng bạn chọn đã được người khác đặt trong khoảng thời gian này. Vui lòng chọn ngày khác hoặc phòng khác!" });

            string bookingCode = $"BK-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}";

            var newBooking = new Booking
            {
                GuestName = req.GuestName,
                GuestPhone = req.GuestPhone,
                GuestEmail = req.GuestEmail,
                BookingCode = bookingCode,
                DepositAmount = req.DepositAmount,
                Status = "Pending", 
                BookingDetails = new List<BookingDetail>() 
            };

            foreach (var roomId in req.SelectedRoomIds)
            {
                var room = await _context
                    .Rooms.Include(r => r.RoomType)
                    .FirstOrDefaultAsync(r => r.Id == roomId);
                if (room != null && room.RoomType != null)
                {
                    // 🚨 ĐÃ FIX LOGIC: XÓA LỆNH room.Status = "Occupied" TẠI ĐÂY!
                    // Lễ tân mới đặt trước thôi, không được đổi trạng thái phòng vật lý thành Đang Ở.
                    // Chỉ khi nào qua trang Arrivals bấm "Check-in" thì phòng mới chuyển sang Occupied.

                    newBooking.BookingDetails.Add(
                        new BookingDetail
                        {
                            RoomId = room.Id,
                            RoomTypeId = room.RoomTypeId,
                            CheckInDate = req.CheckIn,
                            CheckOutDate = req.CheckOut,
                            PricePerNight = room.RoomType.BasePrice,
                        }
                    );
                }
            }

            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync();

            return Ok(
                new { message = "Đặt phòng thành công!", bookingCode = newBooking.BookingCode }
            );
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] UpdateStatusDto req)
        {
            var booking = await _context
                .Bookings.Include(b => b.BookingDetails)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null)
                return NotFound(new { message = "Không tìm thấy đơn!" });

            booking.Status = req.Status;

            foreach (var detail in booking.BookingDetails)
            {
                if (detail.RoomId.HasValue)
                {
                    var room = await _context.Rooms.FindAsync(detail.RoomId.Value);
                    if (room != null)
                    {
                        if (req.Status == "Checked_in")
                            room.Status = "Occupied";
                        else if (req.Status == "Completed" || req.Status == "Cancelled")
                        {
                            room.Status = "Available";
                            if (req.Status == "Completed")
                                room.CleaningStatus = "Dirty";
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật thành công!" });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingDetail(int id)
        {
            var booking = await _context
                .Bookings.Include(b => b.BookingDetails!)
                    .ThenInclude(bd => bd.Room!)
                .Include(b => b.BookingDetails!)
                    .ThenInclude(bd => bd.RoomType!)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null)
                return NotFound();
            return Ok(
                new
                {
                    id = booking.Id,
                    bookingCode = booking.BookingCode,
                    guestName = booking.GuestName,
                    status = booking.Status,
                    details = booking.BookingDetails!.Select(d => new
                    {
                        roomNumber = d.Room?.RoomNumber,
                        roomTypeName = d.RoomType?.Name,
                        checkIn = d.CheckInDate,
                        checkOut = d.CheckOutDate,
                        pricePerNight = d.PricePerNight,
                    }),
                }
            );
        }

        // ==============================================================================
        // DANH SÁCH KHÁCH ĐANG LƯU TRÚ (IN-HOUSE)
        // ==============================================================================
        [HttpGet("in-house")]
        public async Task<IActionResult> GetInHouseGuests()
        {
            var inHouse = await _context
                .Bookings.Include(b => b.BookingDetails!)
                    .ThenInclude(bd => bd.Room!)
                // 🚨 Bao lô cả tiếng Anh lẫn tiếng Việt cho chắc cú
                .Where(b => b.Status == "Checked_in" || b.Status == "Đang ở")
                .Select(b => new
                {
                    id = b.Id,
                    bookingCode = b.BookingCode,
                    guestName = b.GuestName,
                    guestPhone = b.GuestPhone,
                    // Lấy danh sách số phòng khách đang ở
                    roomNumbers = b
                        .BookingDetails.Where(d => d.Room != null)
                        .Select(d => d.Room.RoomNumber)
                        .ToList(),
                    checkInDate = b.BookingDetails.Min(d => d.CheckInDate),
                    checkOutDate = b.BookingDetails.Max(d => d.CheckOutDate),
                })
                .ToListAsync();

            return Ok(inHouse);
        }
    }

    public class UpdateStatusDto
    {
        public string Status { get; set; }
    }
}
