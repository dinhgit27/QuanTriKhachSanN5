using System.ComponentModel.DataAnnotations;

namespace QuanTriKhachSanN5.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; } 
        // ID dịch vụ (khóa chính)

        [Required]
        public string ServiceName { get; set; } 
        // tên dịch vụ (Pizza, Massage...)

        public string ServiceType { get; set; } 
        // loại dịch vụ (Food, Spa, Laundry)

        public decimal Price { get; set; } 
        // giá tiền

        public string Description { get; set; } 
        // mô tả dịch vụ
    }
}