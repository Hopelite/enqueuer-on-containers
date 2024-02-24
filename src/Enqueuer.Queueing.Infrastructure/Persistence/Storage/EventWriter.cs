using Enqueuer.Queueing.Domain.Models;

namespace Enqueuer.Queueing.Infrastructure.Persistence.Storage;

public class EventWriter /*: IEventWriter*/
{
    private readonly SemaphoreSlim _semaphore = new(initialCount: 1, maxCount: 1);

    public async Task ApplyEventsAsync(Group group, CancellationToken cancellationToken)
    {
        await _semaphore.WaitAsync(cancellationToken);

        foreach (var @event in group.DomainEvents.OrderBy(e => e.Timestamp))
        {
            try
            {
                group.Apply(@event);
            }
            catch (Exception ex)
            {
            }
        }

        throw new NotImplementedException();
    }
}
