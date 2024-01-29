using Enqueuer.Queueing.API.Contract.Queries.Models;
using MediatR;

namespace Enqueuer.Queueing.API.Application.Queries.Handlers;

public class GetQueueQueryHandler : IRequestHandler<GetQueueQuery, Queue>
{
    public Task<Queue> Handle(GetQueueQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
