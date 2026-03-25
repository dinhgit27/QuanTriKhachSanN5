using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.DTOs;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Services
{
    public class ReceptionService : IReceptionService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReceptionService> _logger;

        public ReceptionService(ApplicationDbContext context, ILogger<ReceptionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task CheckInBookingAsync(int bookingId, int roomId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null && booking.Status == "Confirmed")
            {
                var detail = await _context.BookingDetails.FirstOrDefaultAsync(bd => bd.BookingId == bookingId);
                if (detail != null)
                {
                    detail.RoomId = roomId;
                    booking.Status = "CheckedIn";
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Checked in booking {BookingId} to room {RoomId}", bookingId, roomId);
                }
                else
                {
                    _logger.LogWarning("No booking detail found for booking {BookingId}", bookingId);
                }
            }
            else
            {
                _logger.LogWarning("Booking {BookingId} not found or not Confirmed", bookingId);
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
            if (service == null)
            {
                _logger.LogError("Service {ServiceId} not found for order in booking {BookingId}", serviceId, bookingId);
                throw new KeyNotFoundException($"Service with ID {serviceId} not found");
            }

            var detail = new Order_Service_Detail
            {
                OrderServiceId = order.Id,
                ServiceId = serviceId,
                Quantity = quantity,
                UnitPrice = service.Price
            };
            _context.OrderServiceDetails.Add(detail);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created order service {OrderId} for booking {BookingId}, service {ServiceId}", order.Id, bookingId, serviceId);
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
            _context.LossAndDamages.Add(damage);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Reported damage {DamageId} for booking {BookingId}", damage.Id, bookingId);
            return damage;
        }

        public async Task<CheckoutDto> CalculateCheckoutAsync(int bookingId)
        {
            var booking = await _context.Bookings.Include(b => b.BookingDetails).FirstOrDefaultAsync(b => b.Id == bookingId);
            
            if (booking == null)
            {
                _logger.LogError("Booking {BookingId} not found for checkout", bookingId);
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found");
            }

            var services = await _context.OrderServices.Where(os => os.BookingId == bookingId)
                .Include(os => os.Details).ThenInclude(d => d.Service).ToListAsync();
            var damages = await _context.LossAndDamages.Where(l => l.BookingId == bookingId).ToListAsync();

            decimal roomTotal = booking.BookingDetails.Sum(bd => bd.Price);
            decimal serviceTotal = services.Sum(os => os.Details.Sum(d => d.Quantity * d.UnitPrice));
            decimal damageTotal = damages.Sum(d => d.FineAmount);
            decimal discount = 0; // Tính từ Voucher nếu có

            var dto = new CheckoutDto
            {
                BookingId = bookingId,
                RoomCharges = roomTotal,
                ServiceCharges = serviceTotal,
                DamageCharges = damageTotal,
                Discounts = discount,
                TotalAmount = roomTotal + serviceTotal + damageTotal - discount
            };

            _logger.LogInformation("Calculated checkout for booking {BookingId}: Total {Total}", bookingId, dto.TotalAmount);
            return dto;
        }
    }
}
