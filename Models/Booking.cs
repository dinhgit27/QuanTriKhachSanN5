using System.ComponentModel.DataAnnotations;

namespace QuanTriKhachSanN5.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GuestId { get; set; }
        public User Guest { get; set; }

        [Required]
        public int RoomId { get; set; }
        public Room Room { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        public string Status { get; set; } // Pending, Confirmed, CheckedIn, CheckedOut, Cancelled

        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public ICollection<Booking_Detail> BookingDetails { get; set; }
    }
}
