using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Models;
using System.Collections.Generic;

namespace QuanTriKhachSanN5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LossAndDamagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LossAndDamagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ======================
        // POST
        // ======================
        [HttpPost]
        public async Task<IActionResult> ReportLoss([FromBody] LossAndDamage report)
        {
            _context.LossAndDamages.Add(report);
            await _context.SaveChangesAsync();

            return Ok(report);
        }

        // ======================
        // GET
        // ======================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LossAndDamage>>> GetAll()
    {
        return await _context.LossAndDamages.ToListAsync();
    }

        // ======================
        // PUT
        // ======================
        [HttpPut("{id}")]
public async Task<IActionResult> Update(int id, LossAndDamage model)
{
    var data = await _context.LossAndDamages.FindAsync(id);

    if (data == null)
    {
        return NotFound();
    }

    data.BookingDetailId = model.BookingDetailId;
    data.RoomInventoryId = model.RoomInventoryId;
    data.Quantity = model.Quantity;
    data.FineAmount = model.FineAmount;
    data.Description = model.Description;
    data.ReportedDate = model.ReportedDate;

    await _context.SaveChangesAsync();

    return Ok(data);
}
[HttpDelete("{id}")]
public async Task<IActionResult> Disable(int id)
{
    var data = await _context.LossAndDamages.FindAsync(id);

    if (data == null)
        return NotFound();

    data.Status = "đã hủy";

    await _context.SaveChangesAsync();

    return Ok("Disabled");
}
[HttpPut("enable/{id}")]
public async Task<IActionResult> Enable(int id)
{
    var data = await _context.LossAndDamages.FindAsync(id);

    if (data == null)
        return NotFound();

    data.Status = "đã ghi nhận";

    await _context.SaveChangesAsync();

    return Ok("Enabled");
}
[HttpPut("status/{id}")]
public async Task<IActionResult> UpdateStatus(int id, string status)
{
    var data = await _context.LossAndDamages.FindAsync(id);

    if (data == null)
        return NotFound();

    data.Status = status;

    await _context.SaveChangesAsync();

    return Ok(data);
}
    }
}