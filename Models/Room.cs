using System.ComponentModel.DataAnnotations;

namespace QuanTriKhachSanN5.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string RoomNumber { get; set; }

        public string Status { get; set; } // e.g., Available, Occupied, Maintenance

        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; }

        // Add other properties as needed
    }
}