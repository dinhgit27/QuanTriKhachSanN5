namespace QUANTRIKHACHSANN5.DTOs.Booking
{
    public class CreateBookingDTO
    {
        public string CustomerName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public List<int> RoomIds { get; set; } 
        public string VoucherCode { get; set; } 
    }
}