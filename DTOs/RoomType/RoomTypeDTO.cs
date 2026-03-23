using System.ComponentModel.DataAnnotations;

namespace QuanTriKhachSanN5.DTOs.RoomType
{
    public class RoomTypeDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal BasePrice { get; set; }

        public int CapacityAdults { get; set; }

        public int CapacityChildren { get; set; }

        // Computed field
        public int TotalRooms { get; set; }
    }
}

