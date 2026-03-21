using QuanTriKhachSanN5.DTOs.RoomType;

namespace QuanTriKhachSanN5.Interfaces;

public interface IRoomTypeService
{
    Task<IEnumerable<RoomTypeDTO>> GetAllAsync();
    Task<RoomTypeDTO?> GetByIdAsync(int id);
    Task<RoomTypeDTO> CreateAsync(CreateRoomTypeDTO dto);
    Task<bool> UpdateAsync(int id, CreateRoomTypeDTO dto);
    Task<bool> DeleteAsync(int id);
}
