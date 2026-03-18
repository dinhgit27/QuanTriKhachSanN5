using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanTriKhachSanN5.Models
{
    [Table("Loss_And_Damages")]
    public class LossAndDamage
    {
        [Key]
        public int id { get; set; }

        public int booking_detail_id { get; set; }

        public int room_inventory_id { get; set; }

        public int quantity { get; set; }

        public decimal penalty_amount { get; set; }

        public string? description { get; set; }

        public DateTime created_at { get; set; }
    }
}