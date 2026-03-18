using System.Collections.Generic;
using System.Threading.Tasks;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Interfaces
{
    public interface IRoomService
    {
        Task<List<Room>> GetRoomsAsync();
        Task<Room> GetRoomByIdAsync(int id);
        Task CreateRoomAsync(Room room);
        Task UpdateRoomStatusAsync(int id, string status);
        // Add other methods as needed
    }
}