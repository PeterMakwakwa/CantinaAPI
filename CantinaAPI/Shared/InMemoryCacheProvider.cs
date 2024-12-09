using Microsoft.Extensions.Caching.Memory;

namespace CantinaAPI.Shared
{
    public class InMemoryCacheProvider : ICachingProvider
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCacheProvider(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> GetObjectFromCache<T>(string key, Func<Task<T>> fetch, TimeSpan expiration)
        {
            if (_memoryCache.TryGetValue(key, out T value))
                return value;

            value = await fetch();
            _memoryCache.Set(key, value, expiration);
            return value;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
