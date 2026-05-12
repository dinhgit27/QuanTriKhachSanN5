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

            // Lấy cả đơn Confirmed hoặc Pending đều cho phép Check-in
            if (booking != null)
            {
                var detail = await _context.BookingDetails.FirstOrDefaultAsync(bd =>
                    bd.BookingId == bookingId
                );
                if (detail != null)
                {
                    // 1. Cập nhật chi tiết đơn: Gán phòng cho khách
                    detail.RoomId = roomId;

                    // 2. Đổi trạng thái Booking thành Đang ở
                    booking.Status = "Checked_in";

                    // 3. 🚨 LOGIC MỚI: TÌM PHÒNG VẬT LÝ VÀ ĐỔI TRẠNG THÁI SANG CÓ KHÁCH 🚨
                    var room = await _context.Rooms.FindAsync(roomId);
                    if (room != null)
                    {
                        room.Status = "Occupied"; // Khớp với model Room.cs của ní
                    }

                    // Lưu tất cả thay đổi vào SQL cùng một lúc
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<Order_Service> OrderServiceAsync(
            int bookingId,
            int serviceId,
            int quantity
        )
        {
            // 1. Tìm chính xác chi tiết phòng của khách
            var detailInfo = await _context.BookingDetails.FirstOrDefaultAsync(bd =>
                bd.BookingId == bookingId
            );
            if (detailInfo == null)
                throw new Exception("Không tìm thấy thông tin lưu trú của khách!");

            // 2. Tạo đơn dịch vụ mới (Khởi tạo TotalAmount = 0 để tránh lỗi 500)
            var order = new Order_Service
            {
                BookingDetailId = detailInfo.Id,
                OrderDate = DateTime.Now,
                Status = "Ordered",
                TotalAmount = 0m,
            };
            _context.OrderServices.Add(order);
            await _context.SaveChangesAsync();

            // 3. Tìm thông tin dịch vụ (giá tiền)
            var service = await _context.Services.FindAsync(serviceId);
            if (service == null)
                throw new Exception("Dịch vụ không tồn tại!");

            // 4. Tạo chi tiết dịch vụ
            var detail = new Order_Service_Detail
            {
                OrderServiceId = order.Id,
                ServiceId = serviceId,
                Quantity = quantity,
                UnitPrice = service.Price,
            };
            _context.OrderServiceDetails.Add(detail);

            // 5. 🚨 ĐÃ FIX LỖI CS0019: Xóa ?? 0m vì service.Price đã là decimal chuẩn
            order.TotalAmount = quantity * service.Price;

            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<LossAndDamage> ReportDamageAsync(
            int bookingId,
            string description,
            decimal fineAmount
        )
        {
            var bookingDetail = await _context.BookingDetails.FirstOrDefaultAsync(bd =>
                bd.BookingId == bookingId
            );
            int detailId = bookingDetail != null ? bookingDetail.Id : 0;

            var damage = new LossAndDamage
            {
                BookingDetailId = detailId,
                Description = description,
                PenaltyAmount = fineAmount,
                CreatedAt = DateTime.Now,
            };

            _context.LossAndDamages.Add(damage);
            await _context.SaveChangesAsync();
            return damage;
        }

        public async Task<CheckoutDto> CalculateCheckoutAsync(int bookingId)
        {
            var booking = await _context
                .Bookings.Include(b => b.BookingDetails)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
                throw new Exception("Booking not found");

            var detailIds = booking.BookingDetails.Select(bd => bd.Id).ToList();

            var services = await _context
                .OrderServices.Where(os =>
                    os.BookingDetailId != null && detailIds.Contains(os.BookingDetailId.Value)
                )
                .ToListAsync();
            var serviceIds = services.Select(s => s.Id).ToList();

            decimal serviceTotal = 0m;
            if (serviceIds.Any())
            {
                var orderDetails = await _context
                    .OrderServiceDetails.Where(d => serviceIds.Contains(d.OrderServiceId))
                    .ToListAsync();

                serviceTotal = orderDetails.Sum(d => d.Quantity * d.UnitPrice);
            }

            var damages = await _context
                .LossAndDamages.Where(l =>
                    l.BookingDetailId != null && detailIds.Contains(l.BookingDetailId.Value)
                )
                .ToListAsync();

            decimal roomTotal = booking.BookingDetails.Sum(bd => bd.PricePerNight);
            decimal damageTotal = damages.Sum(d => d.PenaltyAmount ?? 0m);
            decimal discount = 0m;

            return new CheckoutDto
            {
                BookingId = bookingId,
                RoomCharges = roomTotal,
                ServiceCharges = serviceTotal,
                DamageCharges = damageTotal,
                Discounts = discount,
                TotalAmount = roomTotal + serviceTotal + damageTotal - discount,
            };
        }
    }
}
