// =========================================================================
// APPLICATION DB CONTEXT - TẤT CẢ 25 BẢNG CSDL
// =========================================================================

using Microsoft.EntityFrameworkCore;
using KS_N5.API.Models;

namespace KS_N5.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Module 1: CMS
        public DbSet<Article> Articles { get; set; }
        public DbSet<Article_Category> Article_Categories { get; set; }
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<Review> Reviews { get; set; }

        // Module 2: Booking & CRM
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Booking_Detail> Booking_Details { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Membership> Memberships { get; set; }

        // Module 3: Room Inventory
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> Room_Types { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<Room_Inventory> Room_Inventories { get; set; }
        public DbSet<Room_Image> Room_Images { get; set; }

        // Module 4: Reception
        public DbSet<Service> Services { get; set; }
        public DbSet<Service_Category> Service_Categories { get; set; }
        public DbSet<Order_Service> Order_Services { get; set; }
        public DbSet<Order_Service_Detail> Order_Service_Details { get; set; }
        public DbSet<Loss_And_Damage> Loss_And_Damages { get; set; }

        // Module 5: Payment
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }

        // Module 6: HR & RBAC
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role_Permission> Role_Permissions { get; set; }
        public DbSet<User_Role> User_Roles { get; set; }
        public DbSet<Audit_Log> Audit_Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình quan hệ nếu cần (Foreign Keys)
            // Ví dụ: modelBuilder.Entity<Booking_Detail>().HasOne(bd => bd.Booking).WithMany(b => b.Booking_Details).HasForeignKey(bd => bd.BookingId);
            // Thêm các cấu hình tương tự cho tất cả quan hệ
        }
    }
}