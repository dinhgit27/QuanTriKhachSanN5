// =========================================================================
// MODULE 2: ĐẶT PHÒNG & CRM - MODELS (Bổ sung)
// =========================================================================

namespace KS_N5.API.Models
{
    // Booking.cs đã có, bổ sung Booking_Details
    public class Booking_Detail
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; }
        public int? RoomId { get; set; } // Gán sau khi check-in
        public Room? Room { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal Price { get; set; }
    }

    // Bảng Vouchers: Mã giảm giá
    public class Voucher
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
    }

    // Bảng Memberships: Hạng thẻ thành viên
    public class Membership
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Level { get; set; } // Bronze, Silver, Gold
        public int Points { get; set; }
    }
}