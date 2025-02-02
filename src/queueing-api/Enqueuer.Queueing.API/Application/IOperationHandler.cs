using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Application;

/// <summary>
/// Defines a handler for the incoming requests.
/// </summary>
public interface IOperationHandler<in TRequest> : IRequestHandler<TRequest, IActionResult>
    where TRequest : IRequest<IActionResult>
{
}
