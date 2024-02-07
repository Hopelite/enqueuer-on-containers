using Microsoft.Extensions.DependencyInjection;

namespace Enqueuer.EventBus.Abstractions
{
    public class EventBusBuilder : IEventBusBuilder
    {
        public EventBusBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
