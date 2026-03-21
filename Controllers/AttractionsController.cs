using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using QuanTriKhachSanN5.DTOs.Attraction;
using QuanTriKhachSanN5.DTOs.GoogleMaps;
using QuanTriKhachSanN5.Interfaces;

namespace QuanTriKhachSanN5.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AttractionsController : ControllerBase
{
    private readonly IAttractionService _attractionService;
    private readonly IGoogleMapsService _googleMapsService;

    public AttractionsController(IAttractionService attractionService, IGoogleMapsService googleMapsService)
    {
        _attractionService = attractionService;
        _googleMapsService = googleMapsService;
    }

    /// <summary>
    /// Lấy danh sách tất cả điểm du lịch (không bao gồm đã xóa)
    /// Không yêu cầu quyền đặc biệt - người dùng đã đăng nhập có thể xem
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<AttractionDTO>>> GetAll()
    {
        var attractions = await _attractionService.GetAllAsync();
        return Ok(attractions);
    }

    /// <summary>
    /// Lấy chi tiết điểm du lịch theo ID
    /// </summary>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<AttractionDTO>> GetById(int id)
    {
        var attraction = await _attractionService.GetByIdAsync(id);
        if (attraction is null)
            return NotFound(new { message = "Điểm du lịch không tìm thấy" });

        return Ok(attraction);
    }

    /// <summary>
    /// Tạo điểm du lịch mới
    /// Yêu cầu: Admin hoặc Receptionist
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Receptionist")]
    public async Task<ActionResult<AttractionDTO>> Create([FromBody] CreateAttractionDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _attractionService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Cập nhật điểm du lịch
    /// Yêu cầu: Admin hoặc Receptionist
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Receptionist")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateAttractionDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _attractionService.UpdateAsync(id, dto);
        if (!updated)
            return NotFound(new { message = "Điểm du lịch không tìm thấy" });

        return Ok(new { message = "Cập nhật thành công" });
    }

    /// <summary>
    /// Xóa điểm du lịch (Soft Delete)
    /// Yêu cầu: Admin hoặc Receptionist
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin,Receptionist")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _attractionService.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = "Điểm du lịch không tìm thấy" });

        return Ok(new { message = "Xóa thành công (soft delete)" });
    }

    /// <summary>
    /// Tìm kiếm điểm du lịch từ Google Maps
    /// Yêu cầu: Admin hoặc Receptionist
    /// </summary>
    [HttpGet("search/{query}")]
    [Authorize(Roles = "Admin,Receptionist")]
    public async Task<ActionResult<List<GooglePlaceDetailsDTO>>> SearchAttractions(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest(new { message = "Vui lòng nhập từ khóa tìm kiếm" });

        var attractions = await _attractionService.SearchAttractions(query);
        return Ok(attractions);
    }

    /// <summary>
    /// Lấy danh sách điểm du lịch lân cận từ Google Maps
    /// Không yêu cầu quyền - ai cũng có thể xem
    /// </summary>
    [HttpGet("nearby/{latitude}/{longitude}")]
    [AllowAnonymous]
    public async Task<ActionResult<List<GooglePlaceDetailsDTO>>> GetNearbyAttractions(
        double latitude, 
        double longitude,
        [FromQuery] int radiusMeters = 5000)
    {
        if (latitude < -90 || latitude > 90)
            return BadRequest(new { message = "Latitude không hợp lệ" });

        if (longitude < -180 || longitude > 180)
            return BadRequest(new { message = "Longitude không hợp lệ" });

        if (radiusMeters < 100 || radiusMeters > 50000)
            return BadRequest(new { message = "Radius phải từ 100 đến 50000 mét" });

        var attractions = await _attractionService.GetNearbyAttractions(latitude, longitude, radiusMeters);
        return Ok(attractions);
    }

    /// <summary>
    /// Khôi phục điểm du lịch đã xóa
    /// Yêu cầu: Admin
    /// </summary>
    [HttpPost("{id:int}/restore")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Restore(int id)
    {
        var restored = await _attractionService.RestoreAsync(id);
        if (!restored)
            return NotFound(new { message = "Không tìm thấy điểm du lịch đã xóa" });

        return Ok(new { message = "Khôi phục thành công" });
    }
}
