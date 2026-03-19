using QUANTRIKHACHSANN5.DTOs.Room;
using QUANTRIKHACHSANN5.Models; 

namespace QUANTRIKHACHSANN5.Interfaces
{
    public interface IRoomService
    {
        
        Task<IEnumerable<Room>> GetAvailableRoomsAsync(SearchRoomDTO searchDTO);
    }
}