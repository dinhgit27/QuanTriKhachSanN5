// =========================================================================
// MODULE 4: RECEPTION - INTERFACES
// =========================================================================

using System.Collections.Generic;
using System.Threading.Tasks;
using KS_N5.API.DTOs;

namespace KS_N5.API.Interfaces
{
    public interface IReceptionService
    {
        Task CheckInBookingAsync(int bookingId, int roomId);
        Task<Order_Service> OrderServiceAsync(int bookingId, int serviceId, int quantity);
        Task<Loss_And_Damage> ReportDamageAsync(int bookingId, string description, decimal fineAmount);
        Task<CheckoutDto> CalculateCheckoutAsync(int bookingId);
    }
}