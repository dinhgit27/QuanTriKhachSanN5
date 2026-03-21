using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }
        
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

            // Configure decimal precision and scale for money fields (18, 2)
            modelBuilder.Entity<Amenity>().Property(x => x.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Booking>().Property(x => x.TotalAmount).HasPrecision(18, 2);
            modelBuilder.Entity<Booking_Detail>().Property(x => x.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Invoice>().Property(x => x.DamageFee).HasPrecision(18, 2);
            modelBuilder.Entity<Invoice>().Property(x => x.RoomTotalCost).HasPrecision(18, 2);
            modelBuilder.Entity<Invoice>().Property(x => x.ServicesCost).HasPrecision(18, 2);
            modelBuilder.Entity<Invoice>().Property(x => x.TotalAmount).HasPrecision(18, 2);
            modelBuilder.Entity<Invoice>().Property(x => x.VoucherDiscount).HasPrecision(18, 2);
            modelBuilder.Entity<Loss_And_Damage>().Property(x => x.FineAmount).HasPrecision(18, 2);
            modelBuilder.Entity<Membership>().Property(x => x.DiscountPercent).HasPrecision(18, 2);
            modelBuilder.Entity<Order_Service>().Property(x => x.TotalAmount).HasPrecision(18, 2);
            modelBuilder.Entity<Order_Service_Detail>().Property(x => x.UnitPrice).HasPrecision(18, 2);
            modelBuilder.Entity<Payment>().Property(x => x.AmountPaid).HasPrecision(18, 2);
            modelBuilder.Entity<RoomType>().Property(x => x.BasePrice).HasPrecision(18, 2);
            modelBuilder.Entity<Service>().Property(x => x.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Voucher>().Property(x => x.DiscountAmount).HasPrecision(18, 2);
            modelBuilder.Entity<Voucher>().Property(x => x.DiscountPercent).HasPrecision(18, 2);
        }
    }
}
