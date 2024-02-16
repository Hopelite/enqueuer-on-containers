using Enqueuer.Queueing.Contract.V1.Commands.ViewModels;
using MediatR;

namespace Enqueuer.Queueing.API.Application.Commands;

public class CreateQueueCommand : IRequest<CreatedQueueViewModel>
{
    public CreateQueueCommand(string queueName, long groupId)
    {
        QueueName = queueName;
        GroupId = groupId;
    }

    public string QueueName { get; }

    public long GroupId { get; }
}
