namespace CantinaAPI.Shared
{
    /// <summary>
    /// Represents the cache settings for the application.
    /// This class is used to retrieve and manage the cache duration from configuration settings.
    /// </summary>
    public class CacheSettings
    {
        /// <summary>
        /// Gets or sets the cache duration in minutes as a string.
        /// This value is typically set in the application's configuration (e.g., appsettings.json).
        /// </summary>
        /// <example>"5"</example>
        public required string Cachetime { get; set; }

        /// <summary>
        /// Retrieves the cache duration as a <see cref="TimeSpan"/>.
        /// If the configuration value is invalid or cannot be parsed, it defaults to 5 minutes.
        /// </summary>
        /// <returns>A <see cref="TimeSpan"/> representing the cache duration.</returns>
        /// <remarks>
        /// This method attempts to parse the <see cref="Cachetime"/> property to an integer (minutes).
        /// If parsing fails, it returns a default value of 5 minutes.
        /// </remarks>
        public TimeSpan GetCacheDuration()
        {
            // Attempt to parse the Cachetime property to an integer value representing minutes.
            if (int.TryParse(Cachetime, out int cacheTimeInMinutes))
            {
                return TimeSpan.FromMinutes(cacheTimeInMinutes);
            }

            // Return default cache duration if parsing fails.
            return TimeSpan.FromMinutes(5);
        }
    }
}
