// =========================================================================
// MODULE 3: ROOM INVENTORY - SERVICE
// =========================================================================

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KS_N5.API.Data;
using KS_N5.API.Interfaces;
using KS_N5.API.Models;

namespace KS_N5.API.Services
{
    public class RoomInventoryService : IRoomInventoryService
    {
        private readonly ApplicationDbContext _context;

        public RoomInventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Room>> GetRoomsAsync()
        {
            return await _context.Rooms.Include(r => r.RoomType).ToListAsync();
        }

        public async Task<Room> GetRoomByIdAsync(int id)
        {
            return await _context.Rooms.Include(r => r.RoomType).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateRoomStatusAsync(int roomId, string status)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room != null)
            {
                room.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Amenity>> GetAmenitiesAsync()
        {
            return await _context.Amenities.ToListAsync();
        }

        public async Task<List<Room_Inventory>> GetRoomInventoryAsync(int roomId)
        {
            return await _context.Room_Inventories.Where(ri => ri.RoomId == roomId).Include(ri => ri.Amenity).ToListAsync();
        }
    }
}