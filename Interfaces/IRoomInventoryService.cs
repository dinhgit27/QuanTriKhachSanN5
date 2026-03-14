// =========================================================================
// MODULE 3: ROOM INVENTORY - INTERFACES
// =========================================================================

using System.Collections.Generic;
using System.Threading.Tasks;

namespace KS_N5.API.Interfaces
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