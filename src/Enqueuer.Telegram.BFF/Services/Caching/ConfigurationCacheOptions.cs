namespace Enqueuer.Telegram.BFF.Services.Caching;

public class ConfigurationCacheOptions
{
    public TimeSpan SlidingExpiration { get; set; }

    public TimeSpan AbsoluteExpiration { get; set; }

    public long MaxSize { get; set; }
}
