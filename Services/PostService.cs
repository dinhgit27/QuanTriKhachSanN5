using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.DTOs.Post;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Services;

public class PostService : IPostService
{
    private readonly ApplicationDbContext _db;

    public PostService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<PostDTO> CreateAsync(CreatePostDTO dto)
    {
        var entity = new Post
        {
            Title = dto.Title.Trim(),
            Content = dto.Content.Trim(),
            CategoryId = dto.CategoryId,
        };

        _db.Posts.Add(entity);
        await _db.SaveChangesAsync();

        return await GetByIdAsync(entity.Id)
            ?? throw new InvalidOperationException("Could not map created post.");
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Posts.FindAsync(id);
        if (entity is null)
            return false;

        _db.Posts.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<PostDTO>> GetAllAsync()
    {
        return await _db
            .Posts.AsNoTracking()
            .Include(x => x.Category)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => Map(x))
            .ToListAsync();
    }

    public async Task<PostDTO?> GetByIdAsync(int id)
    {
        var entity = await _db
            .Posts.AsNoTracking()
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity is null ? null : Map(entity);
    }

    public async Task<PostDTO?> GetByRoomTypeIdAsync(int roomTypeId)
    {
        var entity = await _db
            .Posts.AsNoTracking()
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.RoomTypeId == roomTypeId);

        return entity is null ? null : Map(entity);
    }

    public async Task<bool> UpdateAsync(int id, CreatePostDTO dto)
    {
        var entity = await _db.Posts.FindAsync(id);
        if (entity is null)
            return false;

        entity.Title = dto.Title.Trim();
        entity.Content = dto.Content.Trim();
        entity.CategoryId = dto.CategoryId;

        await _db.SaveChangesAsync();
        return true;
    }

    private static PostDTO Map(Post entity) =>
        new()
        {
            Id = entity.Id,
            Title = entity.Title,
            Content = entity.Content,
            CategoryId = entity.CategoryId,
            CategoryName = entity.Category?.Name,
            RoomTypeId = entity.RoomTypeId,
            CreatedAt = entity.CreatedAt,
        };
}
