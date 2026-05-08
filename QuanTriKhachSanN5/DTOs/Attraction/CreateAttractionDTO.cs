using System.ComponentModel.DataAnnotations;

namespace QuanTriKhachSanN5.DTOs.Attraction;

public class CreateAttractionDTO
{
    [Required]
    [MaxLength(250)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(500)]
    public string? Location { get; set; }
}
