// =========================================================================
// MODULE 4: RECEPTION - SERVICE
// =========================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KS_N5.API.Data;
using KS_N5.API.DTOs;
using KS_N5.API.Interfaces;
using KS_N5.API.Models;

namespace KS_N5.API.Services
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
                var detail = await _context.Booking_Details.FirstOrDefaultAsync(bd => bd.BookingId == bookingId);
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
            _context.Order_Services.Add(order);
            await _context.SaveChangesAsync();

            var detail = new Order_Service_Detail
            {
                OrderServiceId = order.Id,
                ServiceId = serviceId,
                Quantity = quantity,
                UnitPrice = (await _context.Services.FindAsync(serviceId)).Price
            };
            _context.Order_Service_Details.Add(detail);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Loss_And_Damage> ReportDamageAsync(int bookingId, string description, decimal fineAmount)
        {
            var damage = new Loss_And_Damage
            {
                BookingId = bookingId,
                Description = description,
                FineAmount = fineAmount,
                ReportedDate = DateTime.Now
            };
            _context.Loss_And_Damages.Add(damage);
            await _context.SaveChangesAsync();
            return damage;
        }

        public async Task<CheckoutDto> CalculateCheckoutAsync(int bookingId)
        {
            // Tính tổng: Tiền phòng + Dịch vụ + Phạt - Giảm giá
            var booking = await _context.Bookings.Include(b => b.Booking_Details).FirstOrDefaultAsync(b => b.Id == bookingId);
            var services = await _context.Order_Services.Where(os => os.BookingId == bookingId)
                .Include(os => os.Details).ThenInclude(d => d.Service).ToListAsync();
            var damages = await _context.Loss_And_Damages.Where(l => l.BookingId == bookingId).ToListAsync();

            decimal roomTotal = booking.Booking_Details.Sum(bd => bd.Price);
            decimal serviceTotal = services.Sum(os => os.Details.Sum(d => d.Quantity * d.UnitPrice));
            decimal damageTotal = damages.Sum(d => d.FineAmount);
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