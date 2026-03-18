using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuanTriKhachSanN5.Attributes;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [Permission("Admin", "Staff")]
        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] Room room)
        {
            await _roomService.CreateRoomAsync(room);
            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }

        [HttpGet]
        public async Task<ActionResult<List<Room>>> GetRooms()
        {
            var rooms = await _roomService.GetRoomsAsync();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null) return NotFound();
            return Ok(room);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateRoomStatus(int id, [FromBody] string status)
        {
            await _roomService.UpdateRoomStatusAsync(id, status);
            return NoContent();
        }

        // Note: BookRoom might belong to BookingsController, not here
        // [Permission("Customer")]
        // [HttpPost("booking")]
        // public IActionResult BookRoom()
        // {
        //     // Implement booking logic here or move to BookingsController
        // }
    }
}