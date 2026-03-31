using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanTriKhachSanN5.Models
{
    public class RoomType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Column("base_price")]
        public decimal BasePrice { get; set; }

        [Column("capacity_adults")]
        public int CapacityAdults { get; set; }

        [Column("capacity_children")]
        public int CapacityChildren { get; set; }

        public string? Description { get; set; }

        [Column("size_sqm")]
        public double? SizeSqm { get; set; } // Diện tích phòng (Để double hoặc decimal cho an toàn)

        [Column("bed_type")]
        public string? BedType { get; set; } // Loại giường

        [Column("view_type")]
        public string? ViewType { get; set; } // View phòng

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        public string? Slug { get; set; }

        public string? Content { get; set; }

        // --- KẾT THÚC PHẦN ĐẮP THÊM ---

        // Navigation properties (Thêm dấu ? để không bị dính lỗi validation)
        public ICollection<Room>? Rooms { get; set; }
    }
}