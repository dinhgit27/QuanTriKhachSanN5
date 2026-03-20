using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.DTOs.Attraction;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Services;

public class AttractionService : IAttractionService
{
    private readonly ApplicationDbContext _db;

    public AttractionService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<AttractionDTO> CreateAsync(CreateAttractionDTO dto)
    {
        var entity = new Attraction
        {
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim(),
            Location = dto.Location?.Trim(),
        };

        _db.Attractions.Add(entity);
        await _db.SaveChangesAsync();

        return Map(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Attractions.FindAsync(id);
        if (entity is null)
            return false;

        _db.Attractions.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<AttractionDTO>> GetAllAsync()
    {
        return await _db.Attractions
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => Map(x))
            .ToListAsync();
    }

    public async Task<AttractionDTO?> GetByIdAsync(int id)
    {
        var entity = await _db.Attractions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity is null ? null : Map(entity);
    }

    public async Task<bool> UpdateAsync(int id, CreateAttractionDTO dto)
    {
        var entity = await _db.Attractions.FindAsync(id);
        if (entity is null)
            return false;

        entity.Name = dto.Name.Trim();
        entity.Description = dto.Description?.Trim();
        entity.Location = dto.Location?.Trim();

        await _db.SaveChangesAsync();
        return true;
    }

    private static AttractionDTO Map(Attraction entity)
        => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Location = entity.Location,
            CreatedAt = entity.CreatedAt,
        };
}
