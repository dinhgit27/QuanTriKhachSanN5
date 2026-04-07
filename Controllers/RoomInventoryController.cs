using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;
using QuanTriKhachSanN5.Data;

namespace QuanTriKhachSanN5.Controllers
{
    // =========================================================
    // DTO: CLASS PHIÊN DỊCH DỮ LIỆU TỪ REACT GỬI LÊN
    // =========================================================
    public class AssignInventoryDto
    {
        public int RoomId { get; set; }
        public int AmenityId { get; set; } // React gửi chữ này
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class RoomInventoryController : ControllerBase
    {
        private readonly IRoomInventoryService _roomService;
        private readonly ApplicationDbContext _context;

        public RoomInventoryController(IRoomInventoryService roomService, ApplicationDbContext context)
        {
            _roomService = roomService;
            _context = context;
        }

        // =========================================================
        // 1. LẤY DANH SÁCH VẬT TƯ ĐỂ ĐỔ VÀO DROPDOWN (React gọi /amenities)
        // =========================================================
        [Authorize(Roles = "Admin,Receptionist")]
        [HttpGet("amenities")]
        public async Task<IActionResult> GetAmenities()
        {
            // React vẫn gọi đường dẫn "amenities", nhưng mình ngầm chui vào kho "Equipments" lấy đồ ra
            var equipments = await _context.Equipments
                                           .Where(e => e.IsActive == true)
                                           .ToListAsync();
            return Ok(equipments);
        }

        // =========================================================
        // 2. LẤY DANH SÁCH TÀI SẢN TRONG 1 PHÒNG (KÈM TÊN VÀ ẢNH)
        // =========================================================
        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        [HttpGet("rooms/{roomId}/inventory")]
        public async Task<IActionResult> GetRoomInventory(int roomId)
        {
            var inventory = await _context.RoomInventories
                .Where(ri => ri.RoomId == roomId)
                .Include(ri => ri.Equipment) // Gắn Equipment vào để lấy tên/giá/ảnh
                .Select(ri => new {
                    id = ri.Id,
                    roomId = ri.RoomId,
                    amenityId = ri.EquipmentId, // Trả về chữ amenityId để React hiểu
                    quantity = ri.Quantity,
                    isActive = ri.IsActive,
                    amenityName = ri.Equipment != null ? ri.Equipment.Name : "Không xác định",
                    
                    // Gói lại thành object amenity cho React hiển thị
                    amenity = new {
                        name = ri.Equipment != null ? ri.Equipment.Name : "Không xác định",
                        price = ri.Equipment != null ? ri.Equipment.DefaultPriceIfLost : 0,
                        imageUrl = ri.Equipment != null ? ri.Equipment.ImageUrl : null
                    }
                })
                .ToListAsync();

            return Ok(inventory);
        }

        // =========================================================
        // 3. GÁN VẬT TƯ VÀO PHÒNG (React gửi POST)
        // =========================================================
        [Authorize(Roles = "Admin")]
        [HttpPost("rooms/{roomId}/inventory")]
        public async Task<IActionResult> AssignEquipmentToRoom(int roomId, [FromBody] AssignInventoryDto dto)
        {
            if (roomId != dto.RoomId) return BadRequest(new { message = "ID phòng không khớp." });

            // Hóa phép từ DTO của React thành Model thực tế của C#
            var newInventory = new Room_Inventory
            {
                RoomId = dto.RoomId,
                EquipmentId = dto.AmenityId, // Chuyển AmenityId -> EquipmentId
                Quantity = dto.Quantity,
                IsActive = true
            };

            _context.RoomInventories.Add(newInventory);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Đã gán vật tư cho phòng thành công!", Data = newInventory });
        }

        // =========================================================
        // 4. XÓA VẬT TƯ KHỎI PHÒNG
        // =========================================================
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            var item = await _context.RoomInventories.FindAsync(id);
            if (item != null)
            {
                _context.RoomInventories.Remove(item);
                await _context.SaveChangesAsync();
            }
            return NoContent(); // Cứ trả về NoContent là React hiểu đã xóa thành công
        }

        // =========================================================
        // 5. KHÔI PHỤC VẬT TƯ BỊ HỎNG (Nút "Đã thay mới")
        // =========================================================
        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestoreInventory(int id)
        {
            try
            {
                var inventory = await _context.RoomInventories.FindAsync(id);
                if (inventory == null) return NotFound(new { message = "Không tìm thấy vật tư này." });

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
        // CÁC API VỀ PHÒNG (GIỮ NGUYÊN KHÔNG ĐỤNG ĐẾN)
        // =========================================================
        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        [HttpGet("rooms")]
        public async Task<IActionResult> GetRooms()
        {
            try { return Ok(await _roomService.GetRoomsAsync()); }
            catch (Exception ex) { return StatusCode(500, new { message = "Lỗi Server: " + ex.Message }); }
        }

        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        [HttpGet("rooms/{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null) return NotFound();
            return Ok(room);
        }

        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        [HttpPut("rooms/{id}/status")]
        public async Task<IActionResult> UpdateRoomStatus(int id, [FromBody] string status)
        {
            await _roomService.UpdateRoomStatusAsync(id, status);
            return NoContent();
        }

        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        [HttpPut("rooms/{id}/mark-clean")]
        public async Task<IActionResult> MarkRoomAsClean(int id)
        {
            try
            {
                var room = await _context.Rooms.FindAsync(id);
                if (room == null) return NotFound(new { message = "Không tìm thấy phòng!" });

                room.Status = "Available";
                room.CleaningStatus = "Clean"; 
                _context.Rooms.Update(room);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Đã dọn phòng sạch sẽ!" });
            }
            catch (Exception ex) { return BadRequest(new { message = "Lỗi SQL: " + ex.Message }); }
        }
    }
}