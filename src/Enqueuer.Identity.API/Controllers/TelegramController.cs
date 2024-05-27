using Enqueuer.EventBus.Abstractions;
using Enqueuer.Identity.API.Filters;
using Enqueuer.Identity.API.Parameters;
using Enqueuer.Identity.Contract.V1.Events;
using Enqueuer.Identity.OAuth;
using Enqueuer.Identity.OAuth.Models;
using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Identity.API.Controllers;

[ApiController]
[AuthorizationExceptionFilter]
[Route("api/[controller]/oauth")]
public class TelegramController(/*IOAuthService authService*/) : ControllerBase
{
    private static readonly Dictionary<string, string> _authorizationCache = new Dictionary<string, string>();
    //private readonly IOAuthService _authService = authService;

    [HttpGet("/")]
    public ContentResult Login()
    {
        var htmlContent = @"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Telegram Login</title>
            </head>
            <body>
                <script async src='https://telegram.org/js/telegram-widget.js?22' data-telegram-login='enqueuer_bot' data-size='large' data-onauth='onTelegramAuth(user)' data-request-access='write'></script>
                <script type='text/javascript'>
                    function onTelegramAuth(user) {
                        alert('Logged in as ' + user.first_name + ' ' + user.last_name + ' (' + user.id + (user.username ? ', @' + user.username : '') + ')');
                    }
                </script>
            </body>
            </html>";

        return new ContentResult
        {
            ContentType = "text/html",
            StatusCode = 200,
            Content = htmlContent
        };
    }

    [HttpGet("authorize")]
    public async Task<IActionResult> AuthorizeAsync([FromQuery] AuthorizeViaTelegramQueryParameters query, CancellationToken cancellationToken)
    {
        // 1) Authorizes the client (must be registered)
        //var authorizationRequest = new AuthorizationRequest(query.ResponseType, query.ClientId, query.RedirectUri, new Scope(query.Scope), query.State);
        //var authorizationResponse = await _authService.AuthorizeAsync(authorizationRequest, cancellationToken);

        // 2) Saves the client authorization in cache

        // 3) If authorized, then redirects user-agent to login page

        return RedirectToAction(nameof(Login));
    }

    [HttpGet("authorize/callback")]
    public async Task<IActionResult> AuthorizeCallbackAsync([FromQuery] TelegramAuthorizationCallbackQueryParameters query, CancellationToken cancellationToken)
    {
        // 4) If user accepts, then retrieves the authorization from cache

        // 5) Returns it to the client's redirect_uri

        throw new NotImplementedException();
    }
}
