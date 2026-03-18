using System.ComponentModel.DataAnnotations;

namespace QuanTriKhachSanN5.Models
{
    public class RoomType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal BasePrice { get; set; }

        public int CapacityAdults { get; set; }

        public int CapacityChildren { get; set; }

        // Navigation properties
        public ICollection<Room> Rooms { get; set; }
    }
}