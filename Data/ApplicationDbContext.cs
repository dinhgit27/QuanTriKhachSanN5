using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Post> Posts => Set<Post>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Optional: seed some sample data.
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Tin tức", Description = "Tin tức và thông báo" },
            new Category { Id = 2, Name = "Hướng dẫn", Description = "Hướng dẫn và tài liệu" }
        );

        modelBuilder.Entity<Post>().HasData(
            new Post
            {
                Id = 1,
                Title = "Chào mừng",
                Content = "Đây là bài viết đầu tiên.",
                CategoryId = 1,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}
