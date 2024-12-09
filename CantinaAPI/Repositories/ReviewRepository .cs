using CantinaAPI.Data;
using CantinaAPI.Models;
using CantinaAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CantinaAPI.Repositories
{
    public class ReviewRepository : Repository<ReviewModel>, IReviewRepository
    {
        public ReviewRepository(CantinaDbContext context) : base(context) { }
        public async Task<IEnumerable<ReviewModel>> GetByItemIdAsync(int itemId)
        {
            return await _context.Reviews
                .Where(r => r.ItemId == itemId)
                .ToListAsync();
        }
    }
}
