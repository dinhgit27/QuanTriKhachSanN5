// =========================================================================
// MODULE 1: CMS - INTERFACES
// =========================================================================

using System.Collections.Generic;
using System.Threading.Tasks;

namespace KS_N5.API.Interfaces
{
    public interface ICMSService
    {
        Task<List<Article>> GetArticlesAsync();
        Task<Article> GetArticleByIdAsync(int id);
        Task CreateArticleAsync(Article article);
        Task<List<Attraction>> GetAttractionsAsync();
        Task<List<Review>> GetReviewsByRoomAsync(int roomId);
    }
}