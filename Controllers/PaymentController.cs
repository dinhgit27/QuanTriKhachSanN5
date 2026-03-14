// =========================================================================
// MODULE 5: PAYMENT - CONTROLLER
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
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("invoices")]
        public async Task<ActionResult<Invoice>> CreateInvoice([FromBody] int bookingId)
        {
            var invoice = await _paymentService.CreateInvoiceAsync(bookingId);
            return Ok(invoice);
        }

        [HttpPost("payments")]
        public async Task<ActionResult<Payment>> ProcessPayment([FromBody] PaymentRequest request)
        {
            var payment = await _paymentService.ProcessPaymentAsync(request.InvoiceId, request.Amount, request.Method);
            return Ok(payment);
        }

        [HttpGet("invoices/{invoiceId}/payments")]
        public async Task<ActionResult<List<Payment>>> GetPayments(int invoiceId)
        {
            var payments = await _paymentService.GetPaymentsByInvoiceAsync(invoiceId);
            return Ok(payments);
        }
    }

    // DTO cho request
    public class PaymentRequest
    {
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } // Cash, Card, etc.
    }
}