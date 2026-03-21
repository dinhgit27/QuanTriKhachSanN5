namespace QuanTriKhachSanN5.DTOs.Attraction;

public class AttractionDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }

    // Google Maps
    public string? GooglePlaceId { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    // Images
    public string? MainImageUrl { get; set; }
    public List<string> ImageUrls { get; set; } = new();

    // Audit
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Soft delete info
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
