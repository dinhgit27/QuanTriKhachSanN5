// =========================================================================
// MODULE 3: ROOM INVENTORY - CONTROLLER
// =========================================================================

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using KS_N5.API.Interfaces;
using KS_N5.API.Models;

namespace KS_N5.API.Controllers
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

        [HttpGet("rooms")]
        public async Task<ActionResult<List<Room>>> GetRooms()
        {
            var rooms = await _roomService.GetRoomsAsync();
            return Ok(rooms);
        }

        [HttpGet("rooms/{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null) return NotFound();
            return Ok(room);
        }

        [HttpPut("rooms/{id}/status")]
        public async Task<IActionResult> UpdateRoomStatus(int id, [FromBody] string status)
        {
            await _roomService.UpdateRoomStatusAsync(id, status);
            return NoContent();
        }

        [HttpGet("amenities")]
        public async Task<ActionResult<List<Amenity>>> GetAmenities()
        {
            var amenities = await _roomService.GetAmenitiesAsync();
            return Ok(amenities);
        }

        [HttpGet("rooms/{roomId}/inventory")]
        public async Task<ActionResult<List<Room_Inventory>>> GetRoomInventory(int roomId)
        {
            var inventory = await _roomService.GetRoomInventoryAsync(roomId);
            return Ok(inventory);
        }
    }
}