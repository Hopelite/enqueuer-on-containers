namespace Enqueuer.Telegram.BFF.Services.Configuration;

public class ConfigurationCacheOptions
{
    public TimeSpan SlidingExpiration { get; set; }

    public TimeSpan AbsoluteExpiration { get; set; }

    public long MaxSize { get; set; }
}
