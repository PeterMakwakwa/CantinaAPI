using CantinaAPI.Models;

namespace CantinaAPI.Repositories.Interfaces
{
    public interface IReviewRepository : IRepository<ReviewModel>
    {
        Task<IEnumerable<ReviewModel>> GetByItemIdAsync(int itemId);
    }
}
