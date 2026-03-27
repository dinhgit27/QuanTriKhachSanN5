// =========================================================================
// MODULE 4: LỄ TÂN & DỊCH VỤ MỞ RỘNG - MODELS
// =========================================================================

namespace QuanTriKhachSanN5.Models
{
    // Bảng Services: Dịch vụ (Spa, gọi món)
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? Unit { get; set; }
        public int Status { get; set; } = 1; // 1 = enable, 0 = disable
        public int CategoryId { get; set; }
        public Service_Category Category { get; set; }
    }

    // Bảng Service_Categories: Danh mục dịch vụ
    public class Service_Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Service> Services { get; set; }
    }

    // Bảng Order_Services: Đơn hàng dịch vụ
    public class Order_Service
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } // Ordered, Delivered, Cancelled
        public int? BookingDetailId { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<Order_Service_Detail> Details { get; set; }
    }

    // Bảng Order_Service_Details: Chi tiết đơn hàng
    public class Order_Service_Detail
    {
        public int Id { get; set; }
        public int OrderServiceId { get; set; }
        public Order_Service OrderService { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    // Bảng Loss_And_Damages: Biên bản phạt hỏng đồ
    public class Loss_And_Damage
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
        public int? BookingDetailId { get; set; }
        public int? RoomInventoryId { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public decimal FineAmount { get; set; }
        public string Status { get; set; } = "đã ghi nhận";
        public DateTime ReportedDate { get; set; }
    }
}
