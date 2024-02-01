using Enqueuer.Queueing.Contract.V1.Commands.ViewModels;
using MediatR;

namespace Enqueuer.Queueing.API.Application.Commands;

public class CreateQueueCommand : IRequest<CreatedQueueViewModel>
{
    public CreateQueueCommand(string queueName, long locationId)
    {
        QueueName = queueName;
        LocationId = locationId;
    }

    public string QueueName { get; }

    public long LocationId { get; }
}
