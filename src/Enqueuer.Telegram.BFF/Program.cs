using Enqueuer.Identity.Contract.V1;
using Enqueuer.Queueing.Contract.V1;
using Enqueuer.Telegram.BFF.Core.Models.Callbacks;
using Enqueuer.Telegram.BFF.Core.Models.Messages;
using Enqueuer.Telegram.BFF.Localization;
using Enqueuer.Telegram.BFF.Messages;
using Enqueuer.Telegram.BFF.Messages.Factories;
using Enqueuer.Telegram.Shared.Localization;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Enqueuer.Telegram.BFF;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddTransient<IMessageHandlersFactory, MessageHandlersFactory>();
        builder.Services.AddScoped<IMessageDistributor, MessageDistributor>();
        builder.Services.AddSingleton<IQueueingClient, QueueingClient>(c => new QueueingClient(new Uri(builder.Configuration.GetConnectionString("QueueingAPI"))));
        builder.Services.AddMessageHandlers();
        builder.Services.AddSingleton<ILocalizationProvider, LocalizationProvider>();
        builder.AddTelegramClient();

        builder.Services.Configure<IdentityClientOptions>(builder.Configuration.GetRequiredSection("IdentityProvider"));
        builder.Services.AddIdentityClient();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

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
                throw new NotImplementedException();
            }

            return Results.Ok();
        });

        app.MapGet("/oauth/callback", async (HttpRequest request, [FromServices] IServiceProvider serviceProvider, CancellationToken cancellationToken) =>
        {
            return Results.Ok();
        });

        app.Run();
    }
}
