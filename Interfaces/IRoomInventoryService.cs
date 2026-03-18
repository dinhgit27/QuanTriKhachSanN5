// =========================================================================
// MODULE 3: ROOM INVENTORY - INTERFACES
// =========================================================================

using System.Collections.Generic;
using System.Threading.Tasks;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Interfaces
{
    public interface IRoomInventoryService
    {
        Task<List<Room>> GetRoomsAsync();
        Task<Room> GetRoomByIdAsync(int id);
        Task UpdateRoomStatusAsync(int roomId, string status);
        Task<List<Amenity>> GetAmenitiesAsync();
        Task<List<Room_Inventory>> GetRoomInventoryAsync(int roomId);
    }
}