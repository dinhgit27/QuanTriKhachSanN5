using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Service> Services { get; set; }
        public DbSet<Service_Category> ServiceCategories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Booking_Detail> BookingDetails { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Article_Category> ArticleCategories { get; set; }
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Order_Service> OrderServices { get; set; }
        public DbSet<Order_Service_Detail> OrderServiceDetails { get; set; }
        public DbSet<Loss_And_Damage> LossAndDamages { get; set; }

        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<Room_Inventory> RoomInventories { get; set; }
        public DbSet<Room_Image> RoomImages { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role_Permission> RolePermissions { get; set; }
        public DbSet<User_Role> UserRoles { get; set; }
        public DbSet<Audit_Log> AuditLogs { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Giải quyết lỗi ĐỎ: Ngắt vòng lặp xóa dây chuyền (Multiple Cascade Paths)
            modelBuilder
                .Entity<Booking_Detail>()
                .HasOne(bd => bd.RoomType)
                .WithMany()
                .HasForeignKey(bd => bd.RoomTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User_Role>().ToTable("User_Roles");

            modelBuilder.Entity<User_Role>().HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder
                .Entity<User_Role>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder
                .Entity<User_Role>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<Role_Permission>().HasKey(x => new { x.RoleId, x.PermissionId });
            // 2. Giải quyết lỗi VÀNG: Định dạng tất cả kiểu thập phân thành decimal(18,2)
            foreach (
                var property in modelBuilder
                    .Model.GetEntityTypes()
                    .SelectMany(t => t.GetProperties())
                    .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?))
            )
            {
                property.SetColumnType("decimal(18,2)");
            }
        }
    }
}
