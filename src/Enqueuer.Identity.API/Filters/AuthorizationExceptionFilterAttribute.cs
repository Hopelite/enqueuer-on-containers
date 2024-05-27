using Enqueuer.Identity.OAuth.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Enqueuer.Identity.API.Filters;

public class AuthorizationExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is not AuthorizationException authorizationException)
        {
            return;
        }

        if (authorizationException is ServerErrorException ex)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);

            var logger = context.HttpContext.RequestServices.GetService<ILogger<AuthorizationExceptionFilterAttribute>>();
            logger?.LogError(ex.InnerException, ex.Message);
        }
    }
}
