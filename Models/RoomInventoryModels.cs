// =========================================================================
// MODULE 3: QUẢN TRỊ QUỸ PHÒNG & VẬT TƯ - MODELS (Bổ sung)
// =========================================================================

namespace QuanTriKhachSanN5.Models
{
    // Rooms.cs và RoomType.cs đã có

    // Bảng Amenities: Danh sách tiện ích/vật tư
    public class Amenity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; } // Nếu tính phí
    }

    // Bảng Room_Inventory: Vật tư trong phòng
    public class Room_Inventory
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public int AmenityId { get; set; }
        public Amenity Amenity { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; } // Available, Damaged
    }

    // Bảng Room_Images: Hình ảnh phòng
    public class Room_Image
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPrimary { get; set; }
    }
}