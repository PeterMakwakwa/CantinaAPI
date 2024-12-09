namespace CantinaAPI.Services.Interfaces
{
    public interface IRateLimitingService
    {
        Task<bool> IsRequestAllowedAsync(string userId);
    }
}
