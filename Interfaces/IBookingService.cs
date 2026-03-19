using QUANTRIKHACHSANN5.DTOs.Booking;

namespace QUANTRIKHACHSANN5.Interfaces
{
    public interface IBookingService
    {
        Task<bool> CreateBookingAsync(CreateBookingDTO bookingDTO);
    }
}