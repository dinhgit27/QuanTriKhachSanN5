namespace KS_N5.DTOs
{
    public class GenerateInvoiceRequestDto
    {
        public int BookingId { get; set; }
    }

    public class ProcessPaymentRequestDto
    {
        public int InvoiceId { get; set; }
        public string PaymentMethod { get; set; } = null!; 
        public decimal AmountPaid { get; set; }
        public string? TransactionId { get; set; } 
    }

    public class M2BookingCostDto
    {
        public decimal RoomTotalCost { get; set; }
        public decimal VoucherDiscount { get; set; }
    }

    public class M4ServiceCostDto
    {
        public decimal ServicesCost { get; set; }
        public decimal DamageFee { get; set; }
    }
}