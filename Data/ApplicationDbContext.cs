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
        
        public DbSet<LossAndDamage> Loss_And_Damages { get; set; }
        public DbSet<OrderService> Order_Services { get; set; }
        public DbSet<OrderServiceDetail> Order_Service_Details { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<OrderService> OrderServices { get; set; }
    }
}