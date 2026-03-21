using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.DTOs.Attraction;
using QuanTriKhachSanN5.DTOs.GoogleMaps;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Services;

public class AttractionService : IAttractionService
{
    private readonly ApplicationDbContext _db;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IGoogleMapsService _googleMapsService;

    public AttractionService(ApplicationDbContext db, ICloudinaryService cloudinaryService, IGoogleMapsService googleMapsService)
    {
        _db = db;
        _cloudinaryService = cloudinaryService;
        _googleMapsService = googleMapsService;
    }

    /// <summary>
    /// Tạo điểm du lịch mới với tích hợp Google Maps và Cloudinary
    /// </summary>
    public async Task<AttractionDTO> CreateAsync(CreateAttractionDTO dto)
    {
        var entity = new Attraction
        {
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim(),
            Location = dto.Location?.Trim(),
            GooglePlaceId = dto.GooglePlaceId,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        // Tải ảnh chính nếu có base64
        if (!string.IsNullOrEmpty(dto.MainImageBase64))
        {
            try
            {
                var (imageUrl, publicId) = await _cloudinaryService.UploadImageFromBase64Async(
                    dto.MainImageBase64, 
                    $"attraction_{Guid.NewGuid()}"
                );
                entity.MainImageUrl = imageUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Image upload failed: {ex.Message}");
            }
        }

        _db.Attractions.Add(entity);
        await _db.SaveChangesAsync();

        // Thêm các ảnh phụ nếu có
        if (dto.ImageUrls != null && dto.ImageUrls.Any())
        {
            foreach (var imageUrl in dto.ImageUrls)
            {
                var image = new AttractionImage
                {
                    AttractionId = entity.Id,
                    ImageUrl = imageUrl,
                    CreatedAt = DateTime.UtcNow
                };
                _db.AttractionImages.Add(image);
            }
            await _db.SaveChangesAsync();
        }

        return Map(entity);
    }

    /// <summary>
    /// Soft Delete - đánh dấu là xóa thay vì xóa thực
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Attractions.FindAsync(id);
        if (entity is null)
            return false;

        // Soft delete version
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Lấy danh sách điểm du lịch (không bao gồm đã xóa)
    /// </summary>
    public async Task<IEnumerable<AttractionDTO>> GetAllAsync()
    {
        return await _db.Attractions
            .Where(x => !x.IsDeleted) // Lọc bỏ soft-deleted items
            .Include(x => x.Images)
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => Map(x))
            .ToListAsync();
    }

    /// <summary>
    /// Lấy chi tiết điểm du lịch theo ID
    /// </summary>
    public async Task<AttractionDTO?> GetByIdAsync(int id)
    {
        var entity = await _db.Attractions
            .Where(x => !x.IsDeleted)
            .Include(x => x.Images.Where(i => !i.IsDeleted))
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity is null ? null : Map(entity);
    }

    /// <summary>
    /// Cập nhật điểm du lịch
    /// </summary>
    public async Task<bool> UpdateAsync(int id, CreateAttractionDTO dto)
    {
        var entity = await _db.Attractions
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        if (entity is null)
            return false;

        entity.Name = dto.Name.Trim();
        entity.Description = dto.Description?.Trim();
        entity.Location = dto.Location?.Trim();
        entity.GooglePlaceId = dto.GooglePlaceId;
        entity.Latitude = dto.Latitude;
        entity.Longitude = dto.Longitude;
        entity.UpdatedAt = DateTime.UtcNow;

        // Cập nhật ảnh chính nếu có
        if (!string.IsNullOrEmpty(dto.MainImageBase64))
        {
            // Xóa ảnh cũ khỏi Cloudinary nếu cần
            // ...
            var (imageUrl, _) = await _cloudinaryService.UploadImageFromBase64Async(
                dto.MainImageBase64,
                $"attraction_{entity.Id}_{Guid.NewGuid()}"
            );
            entity.MainImageUrl = imageUrl;
        }

        await _db.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Khôi phục điểm du lịch đã xóa (undo soft delete)
    /// </summary>
    public async Task<bool> RestoreAsync(int id)
    {
        var entity = await _db.Attractions.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted);
        if (entity is null)
            return false;

        entity.IsDeleted = false;
        entity.DeletedAt = null;
        await _db.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Lấy điểm du lịch lân cận từ Google Maps
    /// </summary>
    public async Task<List<GooglePlaceDetailsDTO>> GetNearbyAttractions(double latitude, double longitude, int radiusMeters = 5000)
    {
        var results = new List<GooglePlaceDetailsDTO>();

        try
        {
            var searchResult = await _googleMapsService.FindNearbyPlacesAsync(
                latitude, longitude, radiusMeters, "tourist_attraction"
            );

            if (searchResult?.Results != null)
            {
                foreach (var place in searchResult.Results)
                {
                    var details = await _googleMapsService.GetPlaceDetailsAsync(place.PlaceId);
                    if (details != null)
                        results.Add(details);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching nearby attractions: {ex.Message}");
        }

        return results;
    }

    /// <summary>
    /// Tìm kiếm điểm du lịch từ Google Maps
    /// </summary>
    public async Task<List<GooglePlaceDetailsDTO>> SearchAttractions(string query)
    {
        var results = new List<GooglePlaceDetailsDTO>();

        try
        {
            var searchResult = await _googleMapsService.SearchPlacesAsync(query);

            if (searchResult?.Results != null)
            {
                foreach (var place in searchResult.Results.Take(10)) // Limit to 10
                {
                    var details = await _googleMapsService.GetPlaceDetailsAsync(place.PlaceId);
                    if (details != null)
                        results.Add(details);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching attractions: {ex.Message}");
        }

        return results;
    }

    /// <summary>
    /// Map entity to DTO
    /// </summary>
    private static AttractionDTO Map(Attraction entity)
        => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Location = entity.Location,
            GooglePlaceId = entity.GooglePlaceId,
            Latitude = entity.Latitude,
            Longitude = entity.Longitude,
            MainImageUrl = entity.MainImageUrl,
            ImageUrls = entity.Images?
                .Where(i => !i.IsDeleted)
                .Select(i => i.ImageUrl)
                .ToList() ?? new List<string>(),
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            IsDeleted = entity.IsDeleted,
            DeletedAt = entity.DeletedAt
        };
}
