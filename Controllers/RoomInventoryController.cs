using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanTriKhachSanN5.Data; // Bắt buộc phải có để xài DB Context
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomInventoryController : ControllerBase
    {
        private readonly IRoomInventoryService _roomService;
        private readonly ApplicationDbContext _context; // Khai báo thêm DB Context

        // Tiêm ApplicationDbContext vào hàm khởi tạo
        public RoomInventoryController(
            IRoomInventoryService roomService,
            ApplicationDbContext context
        )
        {
            _roomService = roomService;
            _context = context;
        }

        // =========================================================
        // API KHÔI PHỤC VẬT TƯ (CHO NÚT "ĐÃ THAY MỚI" CỦA KỸ THUẬT)
        // =========================================================
        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestoreInventory(int id)
        {
            try
            {
                var inventory = await _context.RoomInventories.FindAsync(id);
                if (inventory == null)
                    return NotFound(new { message = "Không tìm thấy vật tư này." });

                // Chốt trạng thái "Hoạt động tốt" (true) vĩnh viễn xuống SQL
                inventory.IsActive = true;
                _context.RoomInventories.Update(inventory);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Đã khôi phục vật tư thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi SQL: " + ex.Message });
            }
        }

        // =========================================================
        // 0. API LẤY TẤT CẢ VẬT TƯ (Bơm máu cho bảng React)
        // =========================================================
        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        [HttpGet]
        public async Task<ActionResult<List<Room_Inventory>>> GetAllInventories()
        {
            var inventories = await _roomService.GetAllInventoriesAsync();
            return Ok(inventories);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventory(
            int id,
            [FromBody] Room_Inventory inventory
        )
        {
            if (id != inventory.Id)
                return BadRequest(new { message = "ID không khớp!" });
            await _roomService.UpdateRoomInventoryAsync(inventory);
            return NoContent();
        }

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
        public async Task<IActionResult> GetRooms()
        {
            try
            {
                var rooms = await _roomService.GetRoomsAsync();
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi Server: " + ex.Message });
            }
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
        public async Task<IActionResult> AssignAmenityToRoom(
            int roomId,
            [FromBody] Room_Inventory inventory
        )
        {
            if (roomId != inventory.RoomId)
                return BadRequest(new { message = "ID phòng không khớp." });

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
        // 2. THÊM HÌNH ẢNH
        // =========================================================
        [Authorize(Roles = "Admin")]
        [HttpPost("roomtypes/{roomTypeId}/images")]
        public async Task<IActionResult> AddRoomImage(int roomTypeId, [FromBody] Room_Image image)
        {
            if (roomTypeId != image.RoomTypeId)
                return BadRequest(new { message = "ID loại phòng không khớp." });

            await _roomService.AddRoomImageAsync(image);

            return Ok(new { Message = "Đã thêm hình ảnh loại phòng thành công!", Data = image });
        }

        // =========================================================
        // API CHUYÊN DỤNG CHO DỌN PHÒNG (HOUSEKEEPING)
        // =========================================================
        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        [HttpPut("rooms/{id}/mark-clean")]
        public async Task<IActionResult> MarkRoomAsClean(int id)
        {
            try
            {
                var room = await _context.Rooms.FindAsync(id);
                if (room == null)
                    return NotFound(new { message = "Không tìm thấy phòng!" });

                // 1. Đổi trạng thái chính thành Phòng trống
                room.Status = "Available";

                // 2. Đổi trạng thái Dọn dẹp thành Sạch sẽ (Clean)
                // 💡 LƯU Ý: Nếu file Models/Room.cs của ní đặt tên biến là khác (VD: HousekeepingStatus)
                // thì ní tự sửa lại chữ CleaningStatus ở dòng dưới cho khớp nha!
                room.CleaningStatus = "Clean";

                _context.Rooms.Update(room);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Đã dọn phòng sạch sẽ!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi SQL: " + ex.Message });
            }
        }
    }
}
