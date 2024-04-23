using Microsoft.Extensions.Hosting;

namespace Enqueuer.Queueing.Infrastructure.Commands.Handling;

public interface ICommandHandler<TAggregate> : IHostedService
{
    /// <summary>
    /// Handles the incoming <paramref name="command"/>.
    /// </summary>
    Task HandleAsync(ICommand command, CancellationToken cancellationToken);
}
