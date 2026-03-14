using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_KhachSan.Models
{
    [Table("Invoices")]
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        public int BookingId { get; set; }
        public decimal RoomTotalCost { get; set; }
        public decimal ServicesCost { get; set; }
        public decimal DamageFee { get; set; }
        public decimal VoucherDiscount { get; set; }
        public decimal TotalAmount { get; set; }
        
        [StringLength(50)]
        public string Status { get; set; } = "Pending"; 
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}