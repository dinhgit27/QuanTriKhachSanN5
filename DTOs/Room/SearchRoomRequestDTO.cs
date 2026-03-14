using System;

namespace KS_N5.API.DTOs.Room
{
    public class SearchRoomRequestDTO
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
    }
}