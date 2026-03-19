using QUANTRIKHACHSANN5.Data;
using QUANTRIKHACHSANN5.DTOs.Booking;
using QUANTRIKHACHSANN5.Interfaces;
using QUANTRIKHACHSANN5.Models;

namespace QUANTRIKhACHSANN5.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateBookingAsync(CreateBookingDTO bookingDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                
                var booking = new Booking
                {
                    CustomerName = bookingDTO.CustomerName,
                    CheckInDate = bookingDTO.CheckInDate,
                    CheckOutDate = bookingDTO.CheckOutDate,
                    VoucherCode = bookingDTO.VoucherCode,
                    TotalAmount = 0 
                };
                
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync(); 

                
                decimal total = 0;
                foreach (var roomId in bookingDTO.RoomIds)
                {
                    var room = await _context.Rooms.FindAsync(roomId);
                    if (room == null) throw new Exception($"Phòng {roomId} không tồn tại");

                    var bookingDetail = new BookingDetail
                    {
                        BookingId = booking.Id,
                        RoomId = roomId,
                        Price = room.Price 
                    };
                    _context.BookingDetails.Add(bookingDetail);
                    total += room.Price;
                }

                
                booking.TotalAmount = total;
                _context.Bookings.Update(booking);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync(); 

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); 
                
                return false;
            }
        }
    }
}