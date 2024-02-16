using Enqueuer.Queueing.Contract.V1.Queries.ViewModels;
using MediatR;

namespace Enqueuer.Queueing.API.Application.Queries;

public class GetQueueQuery : IRequest<Queue>
{
    public GetQueueQuery(long id)
    {
        Id = id;
    }

    public long Id { get; }
}
