using Microsoft.AspNetCore.Mvc;
using QUANTRIKHACHSANN5.DTOs.Promotion;
using QUANTRIKHACHSANN5.Interfaces;

namespace QUANTRIKHACHSANN5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionsController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> CalculateDiscount([FromBody] CalculateDiscountDTO dto)
        {
            var finalAmount = await _promotionService.CalculateFinalAmountAsync(dto);
            return Ok(new { 
                OriginalAmount = dto.OriginalAmount,
                FinalAmount = finalAmount,
                DiscountAmount = dto.OriginalAmount - finalAmount
            });
        }
    }
}