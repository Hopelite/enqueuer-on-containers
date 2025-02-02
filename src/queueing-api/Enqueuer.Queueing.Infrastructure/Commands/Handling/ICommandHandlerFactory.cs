namespace Enqueuer.Queueing.Infrastructure.Commands.Handling;

public interface ICommandHandlerFactory<TAggregate>
{
    /// <summary>
    /// Creates new <see cref="ICommandHandler{TAggregate}"/> for <typeparamref name="TAggregate"/> with the specified <paramref name="aggregateId"/>.
    /// </summary>
    ICommandHandler<TAggregate> CreateCommandHandlerFor(long aggregateId);
}
