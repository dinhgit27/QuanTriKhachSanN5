using System;
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanTriKhachSanN5.Models
{
    [Table("Booking_Details")]
    public class BookingDetail 
    {
        [Key] // Xác định rõ đây là khóa chính
        public int Id { get; set; }

        [Column("booking_id")]
        public int BookingId { get; set; }

        [Column("room_id")]
        public int? RoomId { get; set; }

        [Column("room_type_id")]
        public int? RoomTypeId { get; set; }

        [Column("check_in_date")]
        public DateTime CheckInDate { get; set; }

        [Column("check_out_date")]
        public DateTime CheckOutDate { get; set; }

        [Column("price_per_night", TypeName = "decimal(18,2)")] 
        public decimal PricePerNight { get; set; }

        // Các quan hệ giữ nguyên như cũ
        [ForeignKey("BookingId")]
        public virtual Booking? Booking { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room? Room { get; set; }

        [ForeignKey("RoomTypeId")]
        public virtual RoomType? RoomType { get; set; }
    }
}