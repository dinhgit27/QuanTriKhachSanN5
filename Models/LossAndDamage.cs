using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanTriKhachSanN5.Models
{
    [Table("Loss_And_Damages")]
    public class LossAndDamage
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("booking_detail_id")]
        public int BookingDetailId { get; set; }

        [Column("room_inventory_id")]
        public int RoomInventoryId { get; set; }
        
        // Navigation property để C# tự Join lấy tên vật tư bị hỏng
        public Room_Inventory? RoomInventory { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("penalty_amount")]
        public decimal PenaltyAmount { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("ImageUrl")]
        public string? ImageUrl { get; set; }

        [Column("status")]
        public string Status { get; set; } = "Chưa đền bù"; // Trạng thái: Chưa đền bù / Đã đền bù
    }
}