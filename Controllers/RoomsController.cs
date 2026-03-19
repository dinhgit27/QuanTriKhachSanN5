[HttpPost("find-available")] 
public async Task<IActionResult> FindAvailableRooms([FromBody] SearchRoomDTO searchDTO)
{
    if (searchDTO.CheckInDate >= searchDTO.CheckOutDate)
        return BadRequest("Ngày Check-out phải lớn hơn ngày Check-in.");

    var rooms = await _roomService.GetAvailableRoomsAsync(searchDTO);
    return Ok(rooms);
}