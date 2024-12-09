using CantinaAPI.Models;
using CantinaAPI.Repositories.Interfaces;
using CantinaAPI.Services.Interfaces;
using CantinaAPI.Shared;
using Microsoft.Extensions.Options;

namespace CantinaAPI.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ICachingProvider _cacheProvider;
        private readonly TimeSpan _cacheDuration;

        public ReviewService(IReviewRepository reviewRepository, ICachingProvider cacheProvider, IOptions<CacheSettings> cacheSettings)
        {
            _reviewRepository = reviewRepository;
            _cacheProvider = cacheProvider;
            _cacheDuration = cacheSettings.Value.GetCacheDuration();
        }

        public async Task<IEnumerable<ReviewModel>> GetReviewsByItemIdAsync(int itemId)
        {
            string cacheKey = $"{CantinaHelper.CacheKeyAppName}-review-id:{itemId}";

            return await _cacheProvider.GetObjectFromCache(cacheKey,
                () => _reviewRepository.GetByItemIdAsync(itemId),
                _cacheDuration);
        }
        public async Task AddReviewAsync(ReviewModel review)
        {
            await _reviewRepository.CreateAsync(review);
            await _reviewRepository.SaveAsync();
            _cacheProvider.Remove($"{CantinaHelper.CacheKeyAppName}-review-id:{review.ItemId}");
        }   
    }
}
