using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Queueing.API.Application;

/// <summary>
/// Defines the incoming request with operation.
/// </summary>
public interface IOperation : IRequest<IActionResult>
{
}
