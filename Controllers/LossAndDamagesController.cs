using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LossAndDamagesController : ControllerBase
    {
        private readonly ILossAndDamageService _lossService;

        public LossAndDamagesController(ILossAndDamageService lossService)
        {
            _lossService = lossService;
        }

        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        [HttpPost]
        public async Task<IActionResult> ReportLoss([FromBody] LossAndDamage report)
        {
           try 
            {
                // Bổ sung mặc định nếu React quên gửi
                report.CreatedAt = DateTime.Now;
                if (string.IsNullOrEmpty(report.Status)) report.Status = "Chưa đền bù";

                var createdReport = await _lossService.CreateLossAndDamageAsync(report);
                return Ok(createdReport);
            }
            catch (Exception ex)
            {
                // In ra terminal màn hình đen để anh em mình bắt bệnh ngay lập tức
                Console.WriteLine("============= LỖI POST ĐỀN BÙ =============");
                Console.WriteLine("Lỗi chính: " + ex.Message);
                if (ex.InnerException != null) Console.WriteLine("Chi tiết SQL: " + ex.InnerException.Message);
                
                // Trả về thẳng lỗi lên màn hình React
                return StatusCode(500, new { message = ex.InnerException?.Message ?? ex.Message });
            }
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpGet]
        public async Task<IActionResult> GetAllLossAndDamages()
        {
            try
            {
                var rawData = await _lossService.GetAllLossAndDamagesAsync(); 

                // MAP CHUẨN XÁC VỚI CỘT TRONG SQL CỦA NÍ
                var data = rawData.Select(ld => new {
                    Id = ld.Id,
                    BookingDetailId = ld.BookingDetailId,   // Khớp với booking_detail_id
                    RoomInventoryId = ld.RoomInventoryId,   // Khớp với room_inventory_id
                    Quantity = ld.Quantity,                 // Khớp với quantity
                    PenaltyAmount = ld.PenaltyAmount,       // Khớp với penalty_amount
                    Description = ld.Description,           // Khớp với description
                    CreatedAt = ld.CreatedAt,               // Khớp với created_at
                    ImageUrl = ld.ImageUrl,                 // Khớp với ImageUrl
                    Status = ld.Status                      // Khớp với status
                }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi Server: " + ex.Message });
            }
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LossAndDamage model)
        {
            var updatedData = await _lossService.UpdateLossAndDamageAsync(id, model);
            
            if (updatedData == null)
            {
                return NotFound(new { message = "Không tìm thấy biên bản này!" });
            }

            return Ok(updatedData);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Disable(int id)
        {
            var success = await _lossService.UpdateStatusAsync(id, "Đã hủy");
            
            if (!success) return NotFound();

            return Ok(new { message = "Đã hủy biên bản!" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("enable/{id}")]
        public async Task<IActionResult> Enable(int id)
        {
            var success = await _lossService.UpdateStatusAsync(id, "Chưa đền bù");
            
            if (!success) return NotFound();

            return Ok(new { message = "Đã khôi phục biên bản!" });
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var success = await _lossService.UpdateStatusAsync(id, status);
            
            if (!success) return NotFound();

            return Ok(new { message = "Cập nhật trạng thái thành công!" });
        }
    }
}