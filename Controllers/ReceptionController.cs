// =========================================================================
// MODULE 4: RECEPTION - CONTROLLER
// =========================================================================

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanTriKhachSanN5.DTOs;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Controllers.Disabled
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceptionController : ControllerBase
    {
        private readonly IReceptionService _receptionService;

        public ReceptionController(IReceptionService receptionService)
        {
            _receptionService = receptionService;
        }

        [HttpPost("checkin/{bookingId}")]
        public async Task<IActionResult> CheckIn(int bookingId, [FromBody] int roomId)
        {
            await _receptionService.CheckInBookingAsync(bookingId, roomId);
            return Ok("Checked in successfully");
        }

        [HttpPost("services")]
        public async Task<ActionResult<Order_Service>> OrderService(
            [FromBody] OrderServiceRequest request
        )
        {
            var order = await _receptionService.OrderServiceAsync(
                request.BookingId,
                request.ServiceId,
                request.Quantity
            );
            return Ok(order);
        }

        [HttpPost("damages")]
        public async Task<ActionResult<Loss_And_Damage>> ReportDamage(
            [FromBody] DamageRequest request
        )
        {
            var damage = await _receptionService.ReportDamageAsync(
                request.BookingId,
                request.Description,
                request.FineAmount
            );
            return Ok(damage);
        }

        [HttpGet("checkout/{bookingId}")]
        public async Task<ActionResult<CheckoutDto>> CalculateCheckout(int bookingId)
        {
            var checkout = await _receptionService.CalculateCheckoutAsync(bookingId);
            return Ok(checkout);
        }
    }

    // DTOs cho request
    public class OrderServiceRequest
    {
        public int BookingId { get; set; }
        public int ServiceId { get; set; }
        public int Quantity { get; set; }
    }

    public class DamageRequest
    {
        public int BookingId { get; set; }
        public string Description { get; set; }
        public decimal FineAmount { get; set; }
    }
}
