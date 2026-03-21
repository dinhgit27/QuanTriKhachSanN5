using System.ComponentModel.DataAnnotations;

namespace QuanTriKhachSanN5.DTOs.RoomType;

public class CreateRoomTypeDTO
{
    [Required(ErrorMessage = "Tên loại phòng là bắt buộc")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên phải từ 3 đến 100 ký tự")]
    public string Name { get; set; }

    [StringLength(500, ErrorMessage = "Mô tả không quá 500 ký tự")]
    public string Description { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Giá cơ sở phải lớn hơn 0")]
    public decimal BasePrice { get; set; }

    [Range(1, 20, ErrorMessage = "Sức chứa người lớn phải từ 1 đến 20")]
    public int CapacityAdults { get; set; }

    [Range(0, 10, ErrorMessage = "Sức chứa trẻ em phải từ 0 đến 10")]
    public int CapacityChildren { get; set; }
}
