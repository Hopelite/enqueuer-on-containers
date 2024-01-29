using Enqueuer.Queueing.Contract.V1.Queries.Models;
using MediatR;

namespace Enqueuer.Queueing.API.Application.Queries.Handlers;

public class GetQueueQueryHandler : IRequestHandler<GetQueueQuery, Queue>
{
    public Task<Queue> Handle(GetQueueQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
