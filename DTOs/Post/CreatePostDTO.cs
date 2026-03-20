using System.ComponentModel.DataAnnotations;

namespace QuanTriKhachSanN5.DTOs.Post;

public class CreatePostDTO
{
    [Required]
    [MaxLength(250)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    [Required]
    public int CategoryId { get; set; }
}
