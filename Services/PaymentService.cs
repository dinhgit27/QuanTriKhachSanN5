// =========================================================================
// MODULE 5: PAYMENT - SERVICE
// =========================================================================

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Data;
using QuanTriKhachSanN5.Interfaces;
using QuanTriKhachSanN5.Models;

namespace QuanTriKhachSanN5.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Invoice> CreateInvoiceAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            var invoice = new Invoice
            {
                BookingId = bookingId,
                TotalAmount = 0, // Sẽ tính sau
                Status = "Pending",
                CreatedAt = System.DateTime.Now
            };
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<Payment> ProcessPaymentAsync(int invoiceId, decimal amount, string method)
        {
            var payment = new Payment
            {
                InvoiceId = invoiceId,
                AmountPaid = amount,
                PaymentMethod = method,
                CreatedAt = System.DateTime.Now,
                Status = "Completed"
            };
            _context.Payments.Add(payment);

            // Cập nhật Invoice
            var invoice = await _context.Invoices.FindAsync(invoiceId);
            invoice.Status = "Paid";

            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<List<Payment>> GetPaymentsByInvoiceAsync(int invoiceId)
        {
            return await _context.Payments.Where(p => p.InvoiceId == invoiceId).ToListAsync();
        }
    }
}