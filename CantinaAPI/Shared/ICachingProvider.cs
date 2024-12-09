namespace CantinaAPI.Shared
{
    public interface ICachingProvider
    {
        Task<T> GetObjectFromCache<T>(string key, Func<Task<T>> fetch, TimeSpan expiration);
        void Remove(string key);
    }
}
