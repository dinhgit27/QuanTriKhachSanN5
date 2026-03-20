using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanTriKhachSanN5.Models
{
    [Table("Payments")]
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        
        [StringLength(50)]
        public string PaymentMethod { get; set; } = null!;
        public decimal AmountPaid { get; set; }
        
        [StringLength(100)]
        public string TransactionId { get; set; }
        
        [StringLength(50)]
        public string Status { get; set; } = "Success";
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}