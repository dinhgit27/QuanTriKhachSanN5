using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.DTOs.RoomType;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Services;

public class RoomTypeService : IRoomTypeService
{
    private readonly ApplicationDbContext _db;

    public RoomTypeService(ApplicationDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Lấy danh sách tất cả loại phòng
    /// </summary>
    public async Task<IEnumerable<RoomTypeDTO>> GetAllAsync()
    {
        return await _db.RoomTypes
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => Map(x))
            .ToListAsync();
    }

    /// <summary>
    /// Lấy thông tin loại phòng theo ID
    /// </summary>
    public async Task<RoomTypeDTO?> GetByIdAsync(int id)
    {
        var entity = await _db.RoomTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity is null ? null : Map(entity);
    }

    /// <summary>
    /// Tạo mới loại phòng
    /// </summary>
    public async Task<RoomTypeDTO> CreateAsync(CreateRoomTypeDTO dto)
    {
        var entity = new RoomType
        {
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim() ?? string.Empty,
            BasePrice = dto.BasePrice,
            CapacityAdults = dto.CapacityAdults,
            CapacityChildren = dto.CapacityChildren
        };

        _db.RoomTypes.Add(entity);
        await _db.SaveChangesAsync();

        return Map(entity);
    }

    /// <summary>
    /// Cập nhật thông tin loại phòng
    /// </summary>
    public async Task<bool> UpdateAsync(int id, CreateRoomTypeDTO dto)
    {
        var entity = await _db.RoomTypes.FindAsync(id);
        if (entity is null)
            return false;

        entity.Name = dto.Name.Trim();
        entity.Description = dto.Description?.Trim() ?? string.Empty;
        entity.BasePrice = dto.BasePrice;
        entity.CapacityAdults = dto.CapacityAdults;
        entity.CapacityChildren = dto.CapacityChildren;

        await _db.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Xóa loại phòng
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.RoomTypes.FindAsync(id);
        if (entity is null)
            return false;

        _db.RoomTypes.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }

    private static RoomTypeDTO Map(RoomType entity)
        => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            BasePrice = entity.BasePrice,
            CapacityAdults = entity.CapacityAdults,
            CapacityChildren = entity.CapacityChildren
        };
}
