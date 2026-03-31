using System.ComponentModel.DataAnnotations.Schema;

namespace QuanTriKhachSanN5.Models
{
    [Table("Amenities")]
    public class Amenity
    {
        
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;

        [Column("icon_url")]
        public string? IconUrl { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;
    }
}