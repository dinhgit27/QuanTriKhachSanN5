using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanTriKhachSanN5.Models
{
    public class RoomType
    {
        [Key]
        [Column("user_id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("base_price")]
        public decimal BasePrice { get; set; }

        [Column("capacity_adults")]
        public int CapacityAdults { get; set; }

        [Column("capacity_children")]
        public int CapacityChildren { get; set; }

        // Navigation properties
        public ICollection<Room> Rooms { get; set; }
    }
}
