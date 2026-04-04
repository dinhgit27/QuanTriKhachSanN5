using System.ComponentModel.DataAnnotations;

namespace QuanTriKhachSanN5.Models;

public class Attraction
{
    public int Id { get; set; }

    [Required]
    [MaxLength(250)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(500)]
    public string? Location { get; set; }
    
    [MaxLength(100)]
    public string? GooglePlaceId { get; set; }
    
    public double? Latitude { get; set; }
    
    public double? Longitude { get; set; }
    
    [MaxLength(500)]
    public string? MainImageUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsDeleted { get; set; } = false;
}
