namespace Enqueuer.Queueing.Infrastructure.Commands.Handling;

/// <summary>
/// Defines the manager of <see cref="ICommandHandler<TAggregate>"/> instances.
/// </summary>
public interface ICommandHandlerManager<TAggregate>
{
    /// <summary>
    /// Gets the single active <see cref="ICommandHandler{TAggregate}"/> for <typeparamref name="TAggregate"/> with the specified <paramref name="aggregateId"/>.
    /// </summary>
    /// <remarks>Enforces event writing for single aggregate to single writer.</remarks>
    ValueTask<ICommandHandler<TAggregate>> GetActiveCommandHandlerForAsync(long aggregateId);
}
