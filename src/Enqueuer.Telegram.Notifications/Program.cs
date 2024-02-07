using Enqueuer.Queueing.Contract.V1.Events;
using Enqueuer.Telegram.Notifications.Handlers;
using Enqueuer.Telegram.Notifications.Localization;
using Enqueuer.Telegram.Notifications.Persistence;
using Enqueuer.Telegram.Notifications.Persistence.Entities;
using Enqueuer.Telegram.Notifications.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Enqueuer.Telegram.Notifications;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<NotificationsContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("NotificationsDB"))
            .UseSnakeCaseNamingConvention());

        builder.Services.AddTransient<IChatConfigurationService, ChatConfigurationService>();

        builder.Services.AddRabbitMQClient()
            .AddSubscription<QueueCreatedEvent, QueueCreatedHandler>();

        builder.Services.MigrateDatabase();
        builder.Services.AddSingleton<ILocalizationProvider, LocalizationProvider>();

        builder.AddTelegramClient();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapPut("/chats/{chatId}/language", async (long chatId, [FromBody] ChatNotificationsConfiguration chatConfiguration, [FromServices] IChatConfigurationService configurationService, CancellationToken cancellationToken) =>
        {
            // TODO: add validation whether user has rights to change language via Identity API (requests made on behalf of user)
            if (chatConfiguration == null)
            {
                return Results.BadRequest("Chat's notifications configuration must be provided.");
            }

            await configurationService.ConfigureChatNotificationsAsync(chatConfiguration, cancellationToken);
            return Results.Ok();
        })
        .WithName("Configure Chat's Cotifications")
        .WithOpenApi();

        app.Run();
    }
}
