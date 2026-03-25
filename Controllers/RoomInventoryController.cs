// =========================================================================
// MODULE 3: ROOM INVENTORY - CONTROLLER
// =========================================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Controllers
{
[ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "UpdateInventory")]
    public class RoomInventoryController : ControllerBase
    {
        private readonly IRoomInventoryService _roomService;

        public RoomInventoryController(IRoomInventoryService roomService)
        {
            _roomService = roomService;
        }

[Authorize(Policy = "ViewInventory")]
        [HttpGet("rooms")]
        public async Task<ActionResult<List<Room>>> GetRooms()
        {
            var rooms = await _roomService.GetRoomsAsync();
            return Ok(rooms);
        }

        [Authorize(Policy = "ViewInventory")]
        [HttpGet("rooms/{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null) return NotFound();
            return Ok(room);
        }

        [Authorize(Policy = "UpdateInventory")]
        [HttpPut("rooms/{id}/status")]
        public async Task<IActionResult> UpdateRoomStatus(int id, [FromBody] string status)
        {
            await _roomService.UpdateRoomStatusAsync(id, status);
            return NoContent();
        }

        [Authorize(Policy = "ViewInventory")]
        [HttpGet("amenities")]
        public async Task<ActionResult<List<Amenity>>> GetAmenities()
        {
            var amenities = await _roomService.GetAmenitiesAsync();
            return Ok(amenities);
        }

        // =========================================================
        // 1. GÁN VẬT TƯ / TIỆN ÍCH CHO PHÒNG
        // =========================================================
[Authorize(Roles = "Admin", Policy = "UpdateInventory")]
        [HttpPost("rooms/{roomId}/inventory")]
        public async Task<IActionResult> AssignAmenityToRoom(int roomId, [FromBody] Room_Inventory inventory)
        {
            if (roomId != inventory.RoomId)
                return BadRequest("ID phòng không khớp.");

            // Lưu ý: Nhớ thêm hàm AddRoomInventoryAsync vào IRoomInventoryService và RoomInventoryService của bạn
            // await _roomService.AddRoomInventoryAsync(inventory);
            
            return Ok(new { Message = "Đã gán vật tư/tiện ích cho phòng thành công!", Data = inventory });
        }

[Authorize(Roles = "Admin,Receptionist,Housekeeping", Policy = "ViewInventory")]
        [HttpGet("rooms/{roomId}/inventory")]
        public async Task<ActionResult<List<Room_Inventory>>> GetRoomInventory(int roomId)
        {
            var inventory = await _roomService.GetRoomInventoryAsync(roomId);
            return Ok(inventory);
        }

        // =========================================================
        // 2. THÊM HÌNH ẢNH CHO PHÒNG
        // =========================================================
        [Authorize(Roles = "Admin")]
        [HttpPost("rooms/{roomId}/images")]
        public async Task<IActionResult> AddRoomImage(int roomId, [FromBody] Room_Image image)
        {
            // Lưu ý: Nhớ thêm hàm AddRoomImageAsync vào IRoomInventoryService và RoomInventoryService của bạn
            // await _roomService.AddRoomImageAsync(image);
            
            return Ok(new { Message = "Đã thêm hình ảnh phòng thành công!", Data = image });
        }
    }
}