namespace QuanTriKhachSanN5.DTOs.Attraction;

public class AttractionDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public DateTime CreatedAt { get; set; }
}
