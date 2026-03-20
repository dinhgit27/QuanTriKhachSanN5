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
            _context.Loss_And_Damages.Add(report);
            await _context.SaveChangesAsync();

            return Ok(report);
        }

        // ======================
        // GET
        // ======================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LossAndDamage>>> GetAll()
    {
        return await _context.Loss_And_Damages.ToListAsync();
    }

        // ======================
        // PUT
        // ======================
        [HttpPut("{id}")]
public async Task<IActionResult> Update(int id, LossAndDamage model)
{
    var data = await _context.Loss_And_Damages.FindAsync(id);

    if (data == null)
    {
        return NotFound();
    }

    data.booking_detail_id = model.booking_detail_id;
    data.room_inventory_id = model.room_inventory_id;
    data.quantity = model.quantity;
    data.penalty_amount = model.penalty_amount;
    data.description = model.description;
    data.created_at = model.created_at;

    await _context.SaveChangesAsync();

    return Ok(data);
}
[HttpDelete("{id}")]
public async Task<IActionResult> Disable(int id)
{
    var data = await _context.Loss_And_Damages.FindAsync(id);

    if (data == null)
        return NotFound();

    data.status = "đã hủy";

    await _context.SaveChangesAsync();

    return Ok("Disabled");
}
[HttpPut("enable/{id}")]
public async Task<IActionResult> Enable(int id)
{
    var data = await _context.Loss_And_Damages.FindAsync(id);

    if (data == null)
        return NotFound();

    data.status = "đã ghi nhận";

    await _context.SaveChangesAsync();

    return Ok("Enabled");
}
[HttpPut("status/{id}")]
public async Task<IActionResult> UpdateStatus(int id, string status)
{
    var data = await _context.Loss_And_Damages.FindAsync(id);

    if (data == null)
        return NotFound();

    data.status = status;

    await _context.SaveChangesAsync();

    return Ok(data);
}
    }
}