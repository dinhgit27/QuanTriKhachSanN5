using System.ComponentModel.DataAnnotations.Schema;

namespace QuanTriKhachSanN5.Models
{
    [Table("Order_Services")]
    public class OrderService
    {
        public int id { get; set; }

        public int booking_detail_id { get; set; }

        

        public decimal total_amount { get; set; }

        public string? status { get; set; } = "Pending";
    }
}