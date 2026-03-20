using Microsoft.EntityFrameworkCore;
using QUANTRIKHACHSANN5.Data;
using QUANTRIKHACHSANN5.DTOs.Promotion;
using QUANTRIKHACHSANN5.Interfaces;

namespace QUANTRIKHACHSANN5.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly ApplicationDbContext _context;

        public PromotionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> CalculateFinalAmountAsync(CalculateDiscountDTO dto)
        {
            decimal totalDiscountPercent = 0;

            
            if (!string.IsNullOrEmpty(dto.VoucherCode))
            {
                var voucher = await _context.Vouchers
                    .FirstOrDefaultAsync(v => v.Code == dto.VoucherCode && v.IsActive);
                
                if (voucher != null)
                {
                    totalDiscountPercent += voucher.DiscountPercent;
                }
            }

            
            if (dto.MembershipId.HasValue)
            {
                var membership = await _context.Memberships.FindAsync(dto.MembershipId.Value);
                if (membership != null)
                {
                    totalDiscountPercent += membership.DiscountPercent;
                }
            }

            
            if (totalDiscountPercent > 100) totalDiscountPercent = 100;

            decimal discountAmount = dto.OriginalAmount * (totalDiscountPercent / 100);
            return dto.OriginalAmount - discountAmount;
        }
    }
}