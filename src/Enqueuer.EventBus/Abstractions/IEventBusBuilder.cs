using Microsoft.Extensions.DependencyInjection;

namespace Enqueuer.EventBus.Abstractions
{
    public interface IEventBusBuilder
    {
        IServiceCollection Services { get; }
    }
}
