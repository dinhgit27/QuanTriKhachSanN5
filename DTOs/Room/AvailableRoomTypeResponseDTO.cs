namespace KS_N5.API.DTOs.Room
{
    public class AvailableRoomTypeResponseDTO
    {
        public int RoomTypeId { get; set; }
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public int AvailableCount { get; set; } // Số lượng phòng còn trống
    }
}