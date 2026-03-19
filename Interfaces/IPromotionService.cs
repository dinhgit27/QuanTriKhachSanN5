using QUANTRIKHACHSANN5.DTOs.Promotion;

namespace QUANTRIKHACHSANN5.Interfaces
{
    public interface IPromotionService
    {
        Task<decimal> CalculateFinalAmountAsync(CalculateDiscountDTO dto);
    }
}