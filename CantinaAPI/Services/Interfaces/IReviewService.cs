using CantinaAPI.Models;

namespace CantinaAPI.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewModel>> GetReviewsByItemIdAsync(int itemId);
        Task AddReviewAsync(ReviewModel review);
    }
}
