// =========================================================================
// MODULE 3: ROOM INVENTORY - SERVICE
// =========================================================================

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Services
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
            return await _context.RoomInventories.Where(ri => ri.RoomId == roomId).Include(ri => ri.Amenity).ToListAsync();
        }
    }
}