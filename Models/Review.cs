using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanTriKhachSanN5.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        public int RoomTypeId { get; set; }

        [ForeignKey("RoomTypeId")]
        public RoomType? RoomType { get; set; }

        public int? BookingId { get; set; }

        [ForeignKey("BookingId")]
        public Booking? Booking { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; } = string.Empty;

        // Tính năng nâng cao
        public int? Cleanliness { get; set; } // 1-5 stars
        public int? Comfort { get; set; } // 1-5 stars
        public int? ServiceQuality { get; set; } // 1-5 stars
        public int? ValueForMoney { get; set; } // 1-5 stars

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public bool IsVerified { get; set; } = false; // Xác nhận đã ở trong phòng
    }
}
