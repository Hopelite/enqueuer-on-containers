using Enqueuer.EventBus.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Subscribes bus client to messages of <typeparamref name="TEvent"/> type and adds <typeparamref name="TEventHandler"/> to handle them.
        /// </summary>
        /// <remarks>You can provide multiple <typeparamref name="TEventHandler"/>s for single <typeparamref name="TEvent"/> type.</remarks>
        public static IEventBusBuilder AddSubscription<TEvent, TEventHandler>(this IEventBusBuilder builder)
            where TEvent : IIntegrationEvent
            where TEventHandler : class, IIntegrationEventHandler<TEvent>
        {
            builder.Services.AddKeyedTransient<IIntegrationEventHandler, TEventHandler>(typeof(TEvent));
            builder.Services.Configure<SubscriptionList>(options => options.Add(typeof(TEvent).Name, typeof(TEvent)));

            return builder;
        }
    }
}
