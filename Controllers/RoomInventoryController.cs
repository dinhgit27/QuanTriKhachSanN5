using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomInventoryController : ControllerBase
    {
        private readonly IRoomInventoryService _roomService;

        public RoomInventoryController(IRoomInventoryService roomService)
        {
            _roomService = roomService;
        }

        // =========================================================
        // 0. API LẤY TẤT CẢ VẬT TƯ (Bơm máu cho bảng React)
        // =========================================================
        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        [HttpGet]
        public async Task<ActionResult<List<Room_Inventory>>> GetAllInventories()
        {
            // (Nếu Service của ní chưa có hàm này thì phải qua Service viết thêm nhé)
            var inventories = await _roomService.GetAllInventoriesAsync();
            return Ok(inventories);
        }

        // Bổ sung API Sửa vật tư (Cho nút Sửa bên React)
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventory(int id, [FromBody] Room_Inventory inventory)
        {
            if (id != inventory.Id) return BadRequest(new { message = "ID không khớp!" });
            await _roomService.UpdateRoomInventoryAsync(inventory);
            return NoContent();
        }

        // Bổ sung API Xóa vật tư (Cho nút Xóa bên React)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            await _roomService.DeleteRoomInventoryAsync(id);
            return NoContent();
        }

        // =========================================================
        // CÁC API CŨ CỦA NÍ ĐÃ ĐƯỢC CHUẨN HÓA LẠI
        // =========================================================

        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        [HttpGet("rooms")]
        public async Task<ActionResult<List<Room>>> GetRooms()
        {
            var rooms = await _roomService.GetRoomsAsync();
            return Ok(rooms);
        }

        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        [HttpGet("rooms/{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
                return NotFound();
            return Ok(room);
        }

        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        [HttpPut("rooms/{id}/status")]
        public async Task<IActionResult> UpdateRoomStatus(int id, [FromBody] string status)
        {
            await _roomService.UpdateRoomStatusAsync(id, status);
            return NoContent();
        }

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpGet("amenities")]
        public async Task<ActionResult<List<Amenity>>> GetAmenities()
        {
            var amenities = await _roomService.GetAmenitiesAsync();
            return Ok(amenities);
        }

        // =========================================================
        // 1. GÁN VẬT TƯ / TIỆN ÍCH CHO PHÒNG
        // =========================================================
        [Authorize(Roles = "Admin")]
        [HttpPost("rooms/{roomId}/inventory")]
        public async Task<IActionResult> AssignAmenityToRoom(int roomId, [FromBody] Room_Inventory inventory)
        {
            if (roomId != inventory.RoomId)
                return BadRequest(new { message = "ID phòng không khớp." });

            // ĐÃ MỞ COMMENT ĐỂ LƯU THẬT XUỐNG SQL
            await _roomService.AddRoomInventoryAsync(inventory);

            return Ok(new { Message = "Đã gán vật tư cho phòng thành công!", Data = inventory });
        }

        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        [HttpGet("rooms/{roomId}/inventory")]
        public async Task<ActionResult<List<Room_Inventory>>> GetRoomInventory(int roomId)
        {
            var inventory = await _roomService.GetRoomInventoryAsync(roomId);
            return Ok(inventory);
        }

        // =========================================================
        // 2. THÊM HÌNH ẢNH (Đã sửa lại thành RoomTypeId cho khớp Model và SQL)
        // =========================================================
        [Authorize(Roles = "Admin")]
        [HttpPost("roomtypes/{roomTypeId}/images")] 
        public async Task<IActionResult> AddRoomImage(int roomTypeId, [FromBody] Room_Image image)
        {
            if (roomTypeId != image.RoomTypeId)
                return BadRequest(new { message = "ID loại phòng không khớp." });

            // ĐÃ MỞ COMMENT ĐỂ LƯU THẬT XUỐNG SQL
            await _roomService.AddRoomImageAsync(image);

            return Ok(new { Message = "Đã thêm hình ảnh loại phòng thành công!", Data = image });
        }
    }
}