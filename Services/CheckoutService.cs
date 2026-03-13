using QL_KhachSan.Models;
using QL_KhachSan.DTOs;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace QL_KhachSan.Services
{
    public class CheckoutService
    {
        private readonly QlyKhachSanContext _dbContext;
        private readonly HttpClient _httpClient;

        public CheckoutService(QlyKhachSanContext dbContext, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
        }

        public async Task<Invoice> GenerateInvoiceAsync(int bookingId)
        {
            // 1. Gọi API Module 2 (Thay URL sau)
            var m2Response = await _httpClient.GetAsync($"http://m2-service.local/api/bookings/{bookingId}/costs");
            m2Response.EnsureSuccessStatusCode();
            var m2Data = JsonSerializer.Deserialize<M2BookingCostDto>(
                await m2Response.Content.ReadAsStringAsync(), 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // 2. Gọi API Module 4 (Thay URL sau)
            var m4Response = await _httpClient.GetAsync($"http://m4-service.local/api/reception/{bookingId}/costs");
            m4Response.EnsureSuccessStatusCode();
            var m4Data = JsonSerializer.Deserialize<M4ServiceCostDto>(
                await m4Response.Content.ReadAsStringAsync(), 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // 3. Tính toán tổng tiền
            decimal totalAmount = m2Data!.RoomTotalCost + m4Data!.ServicesCost + m4Data.DamageFee - m2Data.VoucherDiscount;
            totalAmount = totalAmount < 0 ? 0 : totalAmount;

            // 4. Lưu Invoice
            var invoice = new Invoice
            {
                BookingId = bookingId,
                RoomTotalCost = m2Data.RoomTotalCost,
                ServicesCost = m4Data.ServicesCost,
                DamageFee = m4Data.DamageFee,
                VoucherDiscount = m2Data.VoucherDiscount,
                TotalAmount = totalAmount,
                Status = "Pending"
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