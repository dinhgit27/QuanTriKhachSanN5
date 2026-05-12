using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanTriKhachSanN5.Models;

public class Post
{
    public int Id { get; set; }

    [Required]
    [MaxLength(250)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    [Required]
    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }

    public Category? Category { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
