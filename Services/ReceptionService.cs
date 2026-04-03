// =========================================================================
// MODULE 4: RECEPTION - SERVICE (BẢN CHUẨN ĐÃ FIX LỖI 100%)
// =========================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.DTOs;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.API.Services
{
    public class ReceptionService : IReceptionService
    {
        private readonly ApplicationDbContext _context;

        public ReceptionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CheckInBookingAsync(int bookingId, int roomId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null && booking.Status == "Confirmed")
            {
                // Cập nhật Booking_Details với room_id
                var detail = await _context.BookingDetails.FirstOrDefaultAsync(bd => bd.BookingId == bookingId);
                if (detail != null)
                {
                    detail.RoomId = roomId;
                    booking.Status = "CheckedIn";
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<Order_Service> OrderServiceAsync(int bookingId, int serviceId, int quantity)
        {
            var order = new Order_Service
            {
                BookingId = bookingId,
                OrderDate = DateTime.Now,
                Status = "Ordered"
            };
            _context.OrderServices.Add(order);
            await _context.SaveChangesAsync();

            var service = await _context.Services.FindAsync(serviceId);
            if (service == null) throw new Exception("Service not found");

            var detail = new Order_Service_Detail
            {
                OrderServiceId = order.Id,
                ServiceId = serviceId,
                Quantity = quantity,
                UnitPrice = service.Price
            };
            _context.OrderServiceDetails.Add(detail);
            await _context.SaveChangesAsync();

            return order;
        }

        // ĐÃ FIX: Sửa lại tên biến cho chuẩn với Model LossAndDamage mới
        public async Task<LossAndDamage> ReportDamageAsync(int bookingId, string description, decimal fineAmount)
        {
            // Lấy ID chi tiết phòng của khách để gắn biên bản phạt
            var bookingDetail = await _context.BookingDetails.FirstOrDefaultAsync(bd => bd.BookingId == bookingId);
            int detailId = bookingDetail != null ? bookingDetail.Id : 0;

            var damage = new LossAndDamage
            {
                BookingDetailId = detailId,         // Fix lỗi ko có BookingId
                Description = description,
                PenaltyAmount = fineAmount,         // Fix lỗi FineAmount -> PenaltyAmount
                CreatedAt = DateTime.Now            // Fix lỗi ReportedDate -> CreatedAt
            };
            
            _context.LossAndDamages.Add(damage);
            await _context.SaveChangesAsync();
            return damage;
        }

        public async Task<CheckoutDto> CalculateCheckoutAsync(int bookingId)
        {
            // Tính tổng: Tiền phòng + Dịch vụ + Phạt - Giảm giá
            var booking = await _context.Bookings.Include(b => b.BookingDetails).FirstOrDefaultAsync(b => b.Id == bookingId);
            
            if (booking == null) throw new Exception("Booking not found");

            // ĐÃ FIX CẢNH BÁO VÀNG: Thêm dấu chấm hỏi (?. / !.) để an toàn với null
            var services = await _context.OrderServices
                .Where(os => os.BookingId == bookingId)
                .Include(os => os.Details!) 
                .ThenInclude(d => d.Service)
                .ToListAsync();

            // ĐÃ FIX: Tìm LossAndDamage dựa trên danh sách BookingDetailId
            var detailIds = booking.BookingDetails.Select(bd => bd.Id).ToList();
            var damages = await _context.LossAndDamages
                .Where(l => detailIds.Contains(l.BookingDetailId)) 
                .ToListAsync();

            decimal roomTotal = booking.BookingDetails.Sum(bd => bd.Price);
            decimal serviceTotal = services.Sum(os => os.Details?.Sum(d => d.Quantity * d.UnitPrice) ?? 0);
            
            // ĐÃ FIX: Tính tổng phạt theo PenaltyAmount
            decimal damageTotal = damages.Sum(d => d.PenaltyAmount); 
            decimal discount = 0; // Tính từ Voucher nếu có

            return new CheckoutDto
            {
                BookingId = bookingId,
                RoomCharges = roomTotal,
                ServiceCharges = serviceTotal,
                DamageCharges = damageTotal,
                Discounts = discount,
                TotalAmount = roomTotal + serviceTotal + damageTotal - discount
            };
        }
    }
}