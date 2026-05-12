namespace QuanTriKhachSanN5.DTOs.Post;

public class PostDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public DateTime CreatedAt { get; set; }
}
