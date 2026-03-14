// =========================================================================
// MODULE 5: PAYMENT - INTERFACES
// =========================================================================

using System.Collections.Generic;
using System.Threading.Tasks;

namespace KS_N5.API.Interfaces
{
    public interface IPaymentService
    {
        Task<Invoice> CreateInvoiceAsync(int bookingId);
        Task<Payment> ProcessPaymentAsync(int invoiceId, decimal amount, string method);
        Task<List<Payment>> GetPaymentsByInvoiceAsync(int invoiceId);
    }
}