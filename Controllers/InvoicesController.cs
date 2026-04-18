using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InvoicesController(ApplicationDbContext context)
        {
            _context = context;

        [HttpGet("preview/{bookingId}")]
        public async Task<IActionResult> GetInvoicePreview(int bookingId)
        {
            var booking = await _context
                .Bookings.Include(b => b.BookingDetails)
                    .ThenInclude(bd => bd.Room)
                .Include(b => b.BookingDetails)
                    .ThenInclude(bd => bd.RoomType)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
                return NotFound(new { message = "Không tìm thấy đơn!" });

            var detailIds = booking.BookingDetails.Select(bd => bd.Id).ToList();
            decimal totalRoomAmount = 0;
            var roomDetailsList = new List<object>();

            foreach (var detail in booking.BookingDetails)
            {
                int nights = (detail.CheckOutDate.Date - detail.CheckInDate.Date).Days;
                if (nights <= 0)
                    nights = 1;
                decimal roomTotal = detail.PricePerNight * nights;
                totalRoomAmount += roomTotal;

                roomDetailsList.Add(
                    new
                    {
                        roomNumber = detail.Room?.RoomNumber,
                        roomTypeName = detail.RoomType?.Name,
                        nights = nights,
                        pricePerNight = detail.PricePerNight,
                        lineTotal = roomTotal,
                    }
                );
            }

            // Dịch vụ là decimal chuẩn -> KHÔNG CẦN ?? 0m
            decimal totalServiceAmount = await _context
                .OrderServices.Where(os =>
                    os.BookingDetailId != null && detailIds.Contains(os.BookingDetailId.Value)
                )
                .SumAsync(os => os.TotalAmount);

            // 🚨 ĐÃ FIX CS0266: Tiền phạt là decimal? -> BẮT BUỘC CÓ ?? 0m bên trong hàm SumAsync
            decimal totalPenaltyAmount = await _context
                .LossAndDamages.Where(ld =>
                    ld.BookingDetailId != null && detailIds.Contains(ld.BookingDetailId.Value)
                )
                .SumAsync(ld => ld.PenaltyAmount ?? 0m);

            decimal totalServicesAndPenalties = totalServiceAmount + totalPenaltyAmount;
            decimal taxAmount = (totalRoomAmount + totalServicesAndPenalties) * 0.08m;
            decimal finalTotal = totalRoomAmount + totalServicesAndPenalties + taxAmount;

            return Ok(
                new
                {
                    bookingId = booking.Id,
                    bookingCode = booking.BookingCode,
                    guestName = booking.GuestName,
                    totalRoomAmount = totalRoomAmount,
                    totalServiceAmount = totalServicesAndPenalties,
                    discountAmount = 0m,
                    taxAmount = taxAmount,
                    finalTotal = finalTotal,
                    roomDetails = roomDetailsList,
                    note = $"Gồm {totalServiceAmount:N0}đ tiền dịch vụ và {totalPenaltyAmount:N0}đ đền bù",
                }
            );
        }

        [HttpPost("checkout/{bookingId}")]
        public async Task<IActionResult> CheckoutAndCreateInvoice(int bookingId)
        {
            var booking = await _context
                .Bookings.Include(b => b.BookingDetails)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
            if (booking == null)
                return NotFound(new { message = "Không tìm thấy đơn!" });
            if (booking.Status == "Completed")
                return BadRequest(new { message = "Đơn này đã checkout rồi!" });

            var detailIds = booking.BookingDetails.Select(bd => bd.Id).ToList();

            decimal totalRoomAmount = booking.BookingDetails.Sum(d =>
                d.PricePerNight
                * (
                    (d.CheckOutDate.Date - d.CheckInDate.Date).Days == 0
                        ? 1
                        : (d.CheckOutDate.Date - d.CheckInDate.Date).Days
                )
            );

            // Dịch vụ là decimal chuẩn -> KHÔNG CẦN ?? 0m
            decimal totalServiceAmount = await _context
                .OrderServices.Where(os =>
                    os.BookingDetailId != null && detailIds.Contains(os.BookingDetailId.Value)
                )
                .SumAsync(os => os.TotalAmount);

            // 🚨 ĐÃ FIX CS0266: Tiền phạt là decimal? -> BẮT BUỘC CÓ ?? 0m bên trong hàm SumAsync
            decimal totalPenaltyAmount = await _context
                .LossAndDamages.Where(ld =>
                    ld.BookingDetailId != null && detailIds.Contains(ld.BookingDetailId.Value)
                )
                .SumAsync(ld => ld.PenaltyAmount ?? 0m);

            decimal totalServicesCombined = totalServiceAmount + totalPenaltyAmount;
            decimal taxAmount = (totalRoomAmount + totalServicesCombined) * 0.08m;
            decimal finalTotal = totalRoomAmount + totalServicesCombined + taxAmount;

            var newInvoice = new Invoice
            {
                BookingId = booking.Id,
                TotalRoomAmount = totalRoomAmount,
                TotalServiceAmount = totalServicesCombined,
                DiscountAmount = 0m,
                TaxAmount = taxAmount,
                FinalTotal = finalTotal,
                Status = "Paid",
            };
            _context.Invoices.Add(newInvoice);

            booking.Status = "Completed";

            foreach (var detail in booking.BookingDetails)
            {
                if (detail.RoomId.HasValue)
                {
                    var room = await _context.Rooms.FindAsync(detail.RoomId.Value);
                    if (room != null)
                    {
                        room.Status = "Available";
                        room.CleaningStatus = "Dirty";
                    }
                }
            }

            await _context.SaveChangesAsync();
            return Ok(
                new { message = "Checkout và xuất hóa đơn thành công!", invoiceId = newInvoice.Id }
            );
        }
    }
}
