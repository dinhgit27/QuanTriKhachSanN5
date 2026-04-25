using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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
        }
        // ====================================================================
        // LẤY DANH SÁCH TẤT CẢ HÓA ĐƠN (CHO TRANG LỊCH SỬ)
        // ====================================================================
        [HttpGet]
        public async Task<IActionResult> GetAllInvoices()
        {
            try
            {
                // Lấy toàn bộ hóa đơn, sắp xếp hóa đơn mới nhất lên đầu tiên
                var invoices = await _context.Invoices
                    .OrderByDescending(i => i.Id)
                    .Select(i => new 
                    {
                        id = i.Id,
                        bookingId = i.BookingId,
                        finalTotal = i.FinalTotal,
                        status = i.Status
                    })
                    .ToListAsync();

                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi Server: " + ex.Message });
            }
        }

        [HttpGet("preview/{bookingId}")]
        public async Task<IActionResult> GetInvoicePreview(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.BookingDetails!).ThenInclude(bd => bd.Room)
                .Include(b => b.BookingDetails!).ThenInclude(bd => bd.RoomType)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null) return NotFound(new { message = "Không tìm thấy đơn!" });

            var detailIds = booking.BookingDetails.Select(bd => bd.Id).ToList();
            
            // 1. TÍNH TIỀN PHÒNG
            decimal totalRoomAmount = 0;
            var roomDetailsList = new List<object>();

            foreach (var detail in booking.BookingDetails)
            {
                int nights = (detail.CheckOutDate.Date - detail.CheckInDate.Date).Days;
                if (nights <= 0) nights = 1; 
                decimal roomTotal = detail.PricePerNight * nights;
                totalRoomAmount += roomTotal;

                roomDetailsList.Add(new {
                    roomNumber = detail.Room?.RoomNumber,
                    roomTypeName = detail.RoomType?.Name,
                    nights = nights,
                    pricePerNight = detail.PricePerNight,
                    lineTotal = roomTotal
                });
            }

            // 2. LẤY CHI TIẾT DỊCH VỤ (MINI BAR) TÁCH RỜI
            var serviceDetails = await _context.OrderServiceDetails
                .Include(osd => osd.Service)
                .Include(osd => osd.OrderService)
                .Where(osd => osd.OrderService.BookingDetailId != null && detailIds.Contains(osd.OrderService.BookingDetailId.Value))
                .Select(osd => new {
                    serviceName = osd.Service != null ? osd.Service.Name : "Dịch vụ",
                    quantity = osd.Quantity,
                    unitPrice = osd.UnitPrice,
                    total = osd.Quantity * osd.UnitPrice
                })
                .ToListAsync();

            decimal totalServiceAmount = serviceDetails.Sum(s => s.total);

            // 3. LẤY CHI TIẾT ĐỀN BÚ / PHỤ THU TÁCH RỜI
            var penaltyDetails = await _context.LossAndDamages
                .Where(ld => ld.BookingDetailId != null && detailIds.Contains(ld.BookingDetailId.Value))
                .Select(ld => new {
                    itemName = ld.Description ?? "Phạt / Đền bù", 
                    quantity = 1, 
                    
                    penaltyAmount = ld.PenaltyAmount ?? 0m
                })
                .ToListAsync();

            decimal totalPenaltyAmount = penaltyDetails.Sum(p => p.penaltyAmount);

            // 4. TỔNG KẾT
            decimal totalServicesAndPenalties = totalServiceAmount + totalPenaltyAmount;
            decimal taxAmount = (totalRoomAmount + totalServicesAndPenalties) * 0.08m; 
            decimal finalTotal = totalRoomAmount + totalServicesAndPenalties + taxAmount;

            return Ok(new
            {
                bookingId = booking.Id,
                bookingCode = booking.BookingCode,
                guestName = booking.GuestName,
                totalRoomAmount = totalRoomAmount,
                totalServiceAmount = totalServiceAmount, // Gửi riêng lẻ
                totalPenaltyAmount = totalPenaltyAmount, // Gửi riêng lẻ
                discountAmount = 0m,
                taxAmount = taxAmount,
                finalTotal = finalTotal,
                roomDetails = roomDetailsList,
                serviceDetails = serviceDetails,   // 🚨 TRẢ VỀ MẢNG CHI TIẾT CHO REACT MAP RA BẢNG
                penaltyDetails = penaltyDetails,   // 🚨 TRẢ VỀ MẢNG CHI TIẾT ĐỀN BÙ
                note = $"Ghi nhận tiêu thụ hợp lệ"
            });
        }
        // ====================================================================
        // ĐỌC THÔNG TIN HÓA ĐƠN ĐỂ IN (DÙNG LẠI LOGIC CỦA PREVIEW)
        // ====================================================================
        [HttpGet("{invoiceId}")]
        public async Task<IActionResult> GetInvoiceById(int invoiceId)
        {
            var invoice = await _context.Invoices.FindAsync(invoiceId);
            if (invoice == null) return NotFound(new { message = "Không tìm thấy hóa đơn!" });

            // Tuyệt chiêu: Tái sử dụng hàm Preview để gom toàn bộ Phòng, Dịch vụ, Đền bù của Booking này
            return await GetInvoicePreview(invoice.BookingId);
        }
        // ====================================================================
        // NÚT BỎ HÓA ĐƠN (HOÀN TÁC TRẢ PHÒNG)
        // ====================================================================
        [HttpPost("cancel/{invoiceId}")]
        public async Task<IActionResult> CancelInvoice(int invoiceId)
        {
            // 1. Tìm hóa đơn và thông tin đặt phòng đi kèm
            var invoice = await _context.Invoices
                .Include(i => i.Booking)
                .ThenInclude(b => b.BookingDetails)
                .FirstOrDefaultAsync(i => i.Id == invoiceId);

            if (invoice == null) return NotFound(new { message = "Không tìm thấy hóa đơn!" });
            if (invoice.Status == "Cancelled") return BadRequest(new { message = "Hóa đơn này đã bị hủy từ trước!" });

            // 2. Đánh dấu hóa đơn là đã hủy (Không xóa hẳn để còn lưu log đối soát)
            invoice.Status = "Cancelled";

            // 3. Khôi phục trạng thái Đơn đặt phòng
            if (invoice.Booking != null)
            {
                invoice.Booking.Status = "Checked_in"; // Về lại Đang ở

                // 4. Khôi phục trạng thái Phòng vật lý
                foreach (var detail in invoice.Booking.BookingDetails)
                {
                    if (detail.RoomId.HasValue)
                    {
                        var room = await _context.Rooms.FindAsync(detail.RoomId.Value);
                        if (room != null)
                        {
                            room.Status = "Occupied"; // Khóa phòng lại màu đỏ
                            // Xóa cờ báo Dơ (Dirty) vì khách vẫn đang ở
                            if (room.CleaningStatus == "Dirty") room.CleaningStatus = "Clean"; 
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã hủy hóa đơn và khôi phục phòng về danh sách Trả phòng!" });
        }

        [HttpPost("checkout/{bookingId}")]
        public async Task<IActionResult> CheckoutAndCreateInvoice(int bookingId)
        {
            var booking = await _context.Bookings.Include(b => b.BookingDetails).FirstOrDefaultAsync(b => b.Id == bookingId);
            if (booking == null) return NotFound(new { message = "Không tìm thấy đơn!" });
            if (booking.Status == "Completed") return BadRequest(new { message = "Đơn này đã checkout rồi!" });

            var detailIds = booking.BookingDetails.Select(bd => bd.Id).ToList();

            decimal totalRoomAmount = booking.BookingDetails.Sum(d => 
                d.PricePerNight * ((d.CheckOutDate.Date - d.CheckInDate.Date).Days <= 0 ? 1 : (d.CheckOutDate.Date - d.CheckInDate.Date).Days));

            decimal totalServiceAmount = await _context.OrderServices
                .Where(os => os.BookingDetailId != null && detailIds.Contains(os.BookingDetailId.Value))
                .SumAsync(os => os.TotalAmount);
                
            decimal totalPenaltyAmount = await _context.LossAndDamages
                .Where(ld => ld.BookingDetailId != null && detailIds.Contains(ld.BookingDetailId.Value))
                .SumAsync(ld => ld.PenaltyAmount ?? 0m);
            
            decimal totalServicesCombined = totalServiceAmount + totalPenaltyAmount;
            decimal taxAmount = (totalRoomAmount + totalServicesCombined) * 0.08m;
            decimal finalTotal = totalRoomAmount + totalServicesCombined + taxAmount;

            var newInvoice = new Invoice
            {
                BookingId = booking.Id,
                TotalRoomAmount = totalRoomAmount,
                TotalServiceAmount = totalServicesCombined, // Gom chung lưu db theo thiết kế cũ
                DiscountAmount = 0m,
                TaxAmount = taxAmount,
                FinalTotal = finalTotal,
                Status = "Paid" 
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
            return Ok(new { message = "Checkout và xuất hóa đơn thành công!", invoiceId = newInvoice.Id });
        }
    }
}