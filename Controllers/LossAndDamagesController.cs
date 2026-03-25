using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "ManageLossDamages")]
public class LossAndDamagesController : ControllerBase
{
    private readonly ILossAndDamageService _service;

    public LossAndDamagesController(ILossAndDamageService service)
    {
        _service = service;
    }

    // ======================
    // POST
    // ======================
    [HttpPost]
    public async Task<IActionResult> ReportLoss([FromBody] LossAndDamage report)
    {
        var result = await _service.ReportLossAsync(report);
        return result;
    }

    // ======================
    // GET
    // ======================
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return await _service.GetAllAsync();
    }

    // ======================
    // PUT
    // ======================
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, LossAndDamage model)
    {
        return await _service.UpdateAsync(id, model);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Disable(int id)
    {
        return await _service.DisableAsync(id);
    }

    [HttpPut("enable/{id}")]
    public async Task<IActionResult> Enable(int id)
    {
        return await _service.EnableAsync(id);
    }

    [HttpPut("status/{id}")]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        return await _service.UpdateStatusAsync(id, status);
    }
}
