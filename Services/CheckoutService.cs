using QuanTriKhachSanN5.Models;
using QuanTriKhachSanN5.DTOs;
using QuanTriKhachSanN5.Data;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using QuanTriKhachSanN5.Interfaces;

namespace QuanTriKhachSanN5.Services
{
    public class CheckoutService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IReceptionService _receptionService;

        public CheckoutService(ApplicationDbContext dbContext, IReceptionService receptionService)
        {
            _dbContext = dbContext;
            _receptionService = receptionService;
        }

        public async Task<Invoice> GenerateInvoiceAsync(int bookingId)
        {
            // Tận dụng Service tính tiền nội bộ, nhanh và bảo mật hơn gọi HTTP
            var checkoutInfo = await _receptionService.CalculateCheckoutAsync(bookingId);

            // 4. Lưu Invoice
            var invoice = new Invoice
            {
                BookingId = bookingId,
                TotalAmount = checkoutInfo.TotalAmount,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Invoices.Add(invoice);
            await _dbContext.SaveChangesAsync();

            return invoice;
        }

        public async Task<Payment> ProcessPaymentAsync(ProcessPaymentRequestDto request)
        {
            var invoice = await _dbContext.Invoices.FindAsync(request.InvoiceId);
            if (invoice == null) throw new Exception("Invoice not found");
            if (invoice.Status == "Paid") throw new Exception("This invoice is already paid.");

            var payment = new Payment
            {
                InvoiceId = request.InvoiceId,
                PaymentMethod = request.PaymentMethod,
                AmountPaid = request.AmountPaid,
                TransactionId = request.TransactionId,
                Status = "Success"
            };

            _dbContext.Payments.Add(payment);

            invoice.Status = "Paid";
            invoice.UpdatedAt = DateTime.UtcNow;
            
            _dbContext.Invoices.Update(invoice);
            
            await _dbContext.SaveChangesAsync();

            return payment;
        }
    }
}