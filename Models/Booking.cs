namespace QUANTRIKHACHSANN5.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string VoucherCode { get; set; } // Tuỳ chọn

        public ICollection<BookingDetail> BookingDetails { get; set; }
    }
}