using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;
using QuanTriKhachSanN5.DTOs;

namespace QuanTriKhachSanN5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AmenitiesController : ControllerBase
    {
        private readonly IAmenityService _amenityService;

        public AmenitiesController(IAmenityService amenityService)
        {
            _amenityService = amenityService;
        }

        // 1. LẤY TẤT CẢ VẬT TƯ (Cho bảng kho)
        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        [HttpGet]
        public async Task<ActionResult<List<AmenityDto>>> GetAllAmenities()
        {

            var amenities = await _amenityService.GetAllAmenitiesAsync();
            return Ok(amenities);
        }
        // 2. THÊM VẬT TƯ MỚI VÀO KHO
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAmenity([FromBody] Amenity amenity)
        {
            var created = await _amenityService.CreateAmenityAsync(amenity);
            return Ok(new { message = "Thêm vật tư thành công!", data = created });
        }

        // 3. CẬP NHẬT THÔNG TIN VẬT TƯ (Tên, Giá, Ảnh...)
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAmenity(int id, [FromBody] Amenity amenity)
        {
            if (id != amenity.Id) return BadRequest(new { message = "ID không khớp!" });
            
            await _amenityService.UpdateAmenityAsync(amenity);
            return Ok(new { message = "Cập nhật thành công!" });
        }

        // 4. XÓA VẬT TƯ KHỎI KHO
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAmenity(int id)
        {
            // (Thực tế nên kiểm tra xem vật tư này có đang nằm trong phòng nào không trước khi xóa, nhưng tạm thời cứ xóa rẹt rẹt nha)
            await _amenityService.DeleteAmenityAsync(id);
            return Ok(new { message = "Đã xóa vật tư khỏi kho!" });
        }

        // ======================================================
        // 5. API NHẬP KHO (TĂNG SỐ LƯỢNG) - CỰC KỲ QUAN TRỌNG
        // ======================================================
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/import")]
        public async Task<IActionResult> ImportStock(int id, [FromBody] ImportStockRequest request)
        {
            if (request.AddedQuantity <= 0) 
                return BadRequest(new { message = "Số lượng nhập thêm phải lớn hơn 0 ní ơi!" });

            await _amenityService.ImportStockAsync(id, request.AddedQuantity);
            return Ok(new { message = $"Đã nhập thêm {request.AddedQuantity} thành công!" });
        }
    }

    // DTO nhỏ nhắn để hứng dữ liệu từ React gởi lên cho hàm ImportStock
    public class ImportStockRequest
    {
        public int AddedQuantity { get; set; }
    }
}