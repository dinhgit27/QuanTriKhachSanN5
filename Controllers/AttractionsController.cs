using Microsoft.AspNetCore.Mvc;
using QuanTriKhachSanN5.DTOs.Attraction;
using QuanTriKhachSanN5.Interfaces;

namespace QuanTriKhachSanN5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttractionsController : ControllerBase
{
    private readonly IAttractionService _attractionService;

    public AttractionsController(IAttractionService attractionService)
    {
        _attractionService = attractionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AttractionDTO>>> GetAll()
    {
        var attractions = await _attractionService.GetAllAsync();
        return Ok(attractions);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AttractionDTO>> GetById(int id)
    {
        var attraction = await _attractionService.GetByIdAsync(id);
        if (attraction is null)
            return NotFound();

        return Ok(attraction);
    }

    [HttpPost]
    public async Task<ActionResult<AttractionDTO>> Create([FromBody] CreateAttractionDTO dto)
    {
        var created = await _attractionService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateAttractionDTO dto)
    {
        var updated = await _attractionService.UpdateAsync(id, dto);
        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _attractionService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
