using CantinaAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CantinaAPI.CustomActionFilters
{
    /// <summary>
    /// Custom action filter to handle rate limiting.
    /// </summary>
    public class RateLimitFilter : IAsyncActionFilter
    {
        private readonly IRateLimitingService _rateLimitingService;
        private readonly ILogger<RateLimitFilter> _logger;

        public RateLimitFilter(IRateLimitingService rateLimitingService, ILogger<RateLimitFilter> logger)
        {
            _rateLimitingService = rateLimitingService;
            _logger = logger;
        }

        /// <summary>
        /// Checks if the request is allowed based on rate limiting rules.
        /// </summary>
        /// <param name="context">Action executing context.</param>
        /// <param name="next">Action execution delegate.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!await _rateLimitingService.IsRequestAllowedAsync(userId))
            {
                _logger.LogWarning("Rate limit exceeded for user {userId}", userId);
                context.Result = new StatusCodeResult(429);
                return;
            }

            await next();
        }
    }
}
