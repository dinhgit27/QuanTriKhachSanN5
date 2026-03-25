using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "ManageServices")]
public class ServicesController : ControllerBase
{
    private readonly IServiceService _service;

    public ServicesController(IServiceService service)
    {
        _service = service;
    }

[AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Service>>> GetServices()
    {
        return await _service.GetServicesAsync();
    }

[AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<Service>> GetService(int id)
    {
        return await _service.GetServiceAsync(id);
    }

[HttpPost]
    public async Task<ActionResult<Service>> PostService(Service service)
    {
        return await _service.PostServiceAsync(service);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutService(int id, Service service)
    {
        return await _service.PutServiceAsync(id, service);
    }

[HttpDelete("{id}")]
    public async Task<IActionResult> DisableService(int id)
    {
        return await _service.DisableServiceAsync(id);
    }

[HttpPut("enable/{id}")]
    public async Task<IActionResult> EnableService(int id)
    {
        return await _service.EnableServiceAsync(id);
    }
}
