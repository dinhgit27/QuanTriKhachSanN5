using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanTriKhachSanN5.Models
{
    [Table("Invoices")]
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        public int? BookingId { get; set; }
        public decimal? TotalRoomAmount { get; set; }
        public decimal? TotalServiceAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? FinalTotal { get; set; }

        [StringLength(50)]
        public string? Status { get; set; } = "Unpaid";
    }
}
