using Enqueuer.Telegram.BFF.Core.Models.Callbacks;
using Enqueuer.Telegram.BFF.Core.Models.Messages;
using Enqueuer.Telegram.BFF.Messages;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Enqueuer.Telegram.BFF;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.MapPost("/bot", async (Update telegramUpdate, [FromServices] IServiceProvider serviceProvider, CancellationToken cancellationToken) =>
        {
            if (telegramUpdate == null)
            {
                return Results.BadRequest();
            }

            if (telegramUpdate.Type == UpdateType.Message && MessageContext.TryCreate(telegramUpdate.Message!, out var messageContext))
            {
                var distributor = serviceProvider.GetRequiredService<IMessageDistributor>();
                await distributor.DistributeAsync(messageContext, cancellationToken);
            }
            else if (telegramUpdate.Type == UpdateType.CallbackQuery && CallbackContext.TryCreate(telegramUpdate.CallbackQuery!, out var callbackContext))
            {

            }

            return Results.Ok();
        });

        app.Run();
    }
}
