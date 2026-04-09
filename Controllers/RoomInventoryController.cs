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
        public int AmenityId { get; set; }
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

        [Authorize(Roles = "Admin,Receptionist")]
        [HttpGet("amenities")]
        public async Task<IActionResult> GetAmenities()
        {
            var equipments = await _context.Equipments.Where(e => e.IsActive == true).ToListAsync();
            return Ok(equipments);
        }

        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        [HttpGet("rooms/{roomId}/inventory")]
        public async Task<IActionResult> GetRoomInventory(int roomId)
        {
            var inventory = await _context.RoomInventories
                .Where(ri => ri.RoomId == roomId)
                .Include(ri => ri.Equipment) 
                .Select(ri => new {
                    id = ri.Id,
                    roomId = ri.RoomId,
                    amenityId = ri.EquipmentId, 
                    quantity = ri.Quantity,
                    isActive = ri.IsActive,
                    amenityName = ri.Equipment != null ? ri.Equipment.Name : "Không xác định",
                    
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
        // 🚨 NÂNG CẤP 1: KIỂM TRA TỒN KHO TRƯỚC KHI THÊM 1 MÓN
        // =========================================================
        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        [HttpPost("rooms/{roomId}/inventory")]
        public async Task<IActionResult> AssignEquipmentToRoom(int roomId, [FromBody] AssignInventoryDto dto)
        {
            if (roomId != dto.RoomId) return BadRequest(new { message = "ID phòng không khớp." });

            var equipment = await _context.Equipments.FindAsync(dto.AmenityId);
            if (equipment == null) return NotFound(new { message = "Không tìm thấy vật tư trong kho." });

            // LOGIC KIỂM TRA KHO: Tính số lượng đang rảnh rỗi
            int currentlyInUse = await _context.RoomInventories.Where(ri => ri.EquipmentId == dto.AmenityId).SumAsync(ri => ri.Quantity);
            int available = (equipment.TotalQuantity ?? 0) - currentlyInUse;

            if (available < dto.Quantity)
                return BadRequest(new { message = $"Kho không đủ '{equipment.Name}'. Chỉ còn lại: {available} cái." });

            var newInventory = new Room_Inventory
            {
                RoomId = dto.RoomId,
                EquipmentId = dto.AmenityId,
                Quantity = dto.Quantity,
                IsActive = true
            };

            _context.RoomInventories.Add(newInventory);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Đã gán vật tư cho phòng thành công!", Data = newInventory });
        }

        // =========================================================
        // 🚨 NÂNG CẤP 2: API THÊM HÀNG LOẠT (BULK INSERT) SIÊU NHANH
        // =========================================================
        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        [HttpPost("rooms/{roomId}/inventory/bulk")]
        public async Task<IActionResult> AssignBulkEquipmentsToRoom(int roomId, [FromBody] List<AssignInventoryDto> dtos)
        {
            var inventoriesToAdd = new List<Room_Inventory>();

            foreach (var dto in dtos)
            {
                var equipment = await _context.Equipments.FindAsync(dto.AmenityId);
                if (equipment == null) continue;

                // Kiểm tra tồn kho cho từng món
                int currentlyInUse = await _context.RoomInventories.Where(ri => ri.EquipmentId == dto.AmenityId).SumAsync(ri => ri.Quantity);
                int available = (equipment.TotalQuantity ?? 0) - currentlyInUse;

                if (available < dto.Quantity)
                {
                    return BadRequest(new { message = $"Kho không đủ '{equipment.Name}'. Chỉ còn: {available} cái." });
                }

                inventoriesToAdd.Add(new Room_Inventory
                {
                    RoomId = roomId,
                    EquipmentId = dto.AmenityId,
                    Quantity = dto.Quantity,
                    IsActive = true
                });
            }

            if (!inventoriesToAdd.Any())
                return BadRequest(new { message = "Không có vật tư hợp lệ nào để thêm." });

            // AddRange giúp lưu 100 món đồ chỉ với 1 thao tác xuống DB, cực kỳ tối ưu!
            _context.RoomInventories.AddRange(inventoriesToAdd);
            await _context.SaveChangesAsync(); 

            return Ok(new { Message = $"Đã thêm thành công {inventoriesToAdd.Count} vật tư vào phòng!" });
        }

        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            var item = await _context.RoomInventories.FindAsync(id);
            if (item != null)
            {
                _context.RoomInventories.Remove(item);
                await _context.SaveChangesAsync();
            }
            return NoContent(); 
        }

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
        // CÁC API VỀ PHÒNG (GIỮ NGUYÊN)
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