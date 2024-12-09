using CantinaAPI.Services.Interfaces;
using CantinaAPI.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CantinaAPI.Services
{
    /// <summary>
    /// Service for rate limiting requests.
    /// </summary>
    public class RateLimitingService : IRateLimitingService
    {
        private readonly int _requestsPerSecond;
        private readonly int _seconds;
        private readonly ICachingProvider _cachingProvider;
        private readonly ILogger<RateLimitingService> _logger;
        private const int DefaultRequestsPerSecond = 4;
        private const int DefaultSeconds = 1;

        public RateLimitingService(IConfiguration configuration, ICachingProvider cachingProvider, ILogger<RateLimitingService> logger)
        {
            _requestsPerSecond = GetConfigValue(configuration, "RateLimit:RequestsPerSecond", DefaultRequestsPerSecond);
            _seconds = GetConfigValue(configuration, "RateLimit:Seconds", DefaultSeconds);
            _cachingProvider = cachingProvider;
            _logger = logger;

            if (_requestsPerSecond == DefaultRequestsPerSecond || _seconds == DefaultSeconds)
            {
                _logger.LogWarning($"Rate limit configuration is missing or invalid. Using default values: RequestsPerSecond = {DefaultRequestsPerSecond}, Seconds = {DefaultSeconds}");
            }
        }

        /// <summary>
        /// Determines if a request is allowed based on the rate limit.
        /// </summary>
        /// <param name="userId">The user ID making the request.</param>
        /// <returns>True if the request is allowed, otherwise false.</returns>
        public async Task<bool> IsRequestAllowedAsync(string userId)
        {
            var cacheKey = $"rate_limit_{userId}";
            var lastRequestTime = await _cachingProvider.GetObjectFromCache<DateTime?>(cacheKey, async () => null, TimeSpan.FromSeconds(_seconds));

            if (lastRequestTime == null || DateTime.UtcNow - lastRequestTime > TimeSpan.FromSeconds(_seconds))
            {
                await _cachingProvider.GetObjectFromCache(cacheKey, async () => DateTime.UtcNow, TimeSpan.FromSeconds(_seconds));
                return true;
            }

            return false;
        }

        private int GetConfigValue(IConfiguration configuration, string key, int defaultValue)
        {
            return configuration.GetValue<int?>(key) ?? defaultValue;
        }
    }
}