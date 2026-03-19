using Microsoft.EntityFrameworkCore;
using QUANTRIKHACHSANN5.Data;
using QUANTRIKHACHSANN5.DTOs.Room;
using QUANTRIKHACHSANN5.Interfaces;
using QUANTRIKHACHSANN5.Models;

namespace QUANTRIKHACHSANN5.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _context;

        public RoomService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(SearchRoomDTO searchDTO)
        {
            
            var bookedRoomIds = await _context.BookingDetails
                .Include(bd => bd.Booking)
                .Where(bd => bd.Booking.CheckInDate < searchDTO.CheckOutDate 
                          && bd.Booking.CheckOutDate > searchDTO.CheckInDate)
                .Select(bd => bd.RoomId)
                .Distinct()
                .ToListAsync();

            
            var availableRooms = await _context.Rooms
                .Where(r => !bookedRoomIds.Contains(r.Id))
                .ToListAsync();

            return availableRooms;
        }
    }
}