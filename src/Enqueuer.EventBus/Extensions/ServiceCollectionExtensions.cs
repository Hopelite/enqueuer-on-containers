using Enqueuer.EventBus.Abstractions;
using System;
using System.Linq;

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

        /// <summary>
        /// Subscribes bus client to messages of <paramref name="eventType"/> type and adds <paramref name="handlerType"/> to handle them.
        /// </summary>
        /// <remarks>You can provide multiple <paramref name="handlerType"/>s for single <paramref name="eventType"/> type.</remarks>
        public static IEventBusBuilder AddSubscription(this IEventBusBuilder builder, Type eventType, Type handlerType)
        {
            builder.Services.AddKeyedTransient(typeof(IIntegrationEventHandler), eventType, handlerType);
            builder.Services.Configure<SubscriptionList>(options => options.Add(eventType.Name, eventType));

            return builder;
        }

        /// <summary>
        /// Registers all non-abstract implementations of the <see cref="IIntegrationEventHandler{T}"/> in all referenced assemblies.
        /// </summary>
        public static IEventBusBuilder SubscribeAllHandlers(this IEventBusBuilder builder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes()
                    .Where(type => type.IsClass && !type.IsAbstract && typeof(IIntegrationEventHandler).IsAssignableFrom(type));

                foreach (var handlerType in types)
                {
                    // Get all interfaces implemented by the handler type that are closed types of IIntegrationEventHandler<>
                    var handlerInterfaces = handlerType.GetInterfaces()
                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>));

                    foreach (var handlerInterface in handlerInterfaces)
                    {
                        // Extract the event type T from the interface IIntegrationEventHandler<T>
                        var eventType = handlerInterface.GetGenericArguments().First();
                        builder.AddSubscription(eventType, handlerType);
                    }
                }
            }

            return builder;
        }
    }
}
