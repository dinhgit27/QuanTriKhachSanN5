using System.ComponentModel.DataAnnotations.Schema;

namespace QuanTriKhachSanN5.Models
{
    [Table("Room_Inventory")]
    public class Room_Inventory
    {
        public int Id { get; set; }

        [Column("room_id")]
        public int RoomId { get; set; }
        public Room? Room { get; set; }

        [Column("EquipmentId")] // Khớp với tên EquipmentId trong SQL của ní
        public int AmenityId { get; set; }
        public Amenity? Amenity { get; set; }

        public int Quantity { get; set; }

        [Column("price_if_lost")]
        public decimal PriceIfLost { get; set; }

        public string? Note { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("item_type")]
        public string? ItemType { get; set; }
    }
}
