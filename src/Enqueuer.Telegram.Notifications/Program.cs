using Enqueuer.Identity.Contract.V1;
using Enqueuer.Telegram.Notifications.Contract.V1.Models;
using Enqueuer.Telegram.Notifications.Localization;
using Enqueuer.Telegram.Notifications.Persistence;
using Enqueuer.Telegram.Notifications.Services;
using Enqueuer.Telegram.Shared.Localization;
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
            .SubscribeAllHandlers();

        builder.Services.AddSingleton<ILocalizationProvider, LocalizationProvider>();

        builder.Services.Configure<IdentityClientOptions>(builder.Configuration.GetRequiredSection("IdentityProvider"))
                .AddIdentityClient();

        builder.AddTelegramClient();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // TODO: create certificates for API
        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        app.UseAuthorization();

        app.MapGet("/chats/{chatId}", async (long chatId, [FromQuery(Name = "language_code")] string? languageCode, [FromServices] IChatConfigurationService configurationService, CancellationToken cancellationToken) =>
        {
            var chatConfiguration = await configurationService.GetChatConfigurationAsync(chatId, languageCode, cancellationToken);
            return Results.Ok(chatConfiguration);
        })
        .WithName("Get Chat Notification Settings")
        .WithOpenApi();

        app.MapPut("/chats/{chatId}", async (long chatId, [FromBody] ChatNotificationsConfiguration chatConfiguration, [FromServices] IChatConfigurationService configurationService, CancellationToken cancellationToken) =>
        {
            // TODO: add validation whether user has rights to change language via Identity API (requests made on behalf of user)
            if (chatConfiguration == null)
            {
                return Results.BadRequest("Chat's notifications configuration must be provided.");
            }

            await configurationService.ConfigureChatNotificationsAsync(chatConfiguration, cancellationToken);
            return Results.Ok();
        })
        .WithName("Configure Chat Notification Settings")
        .WithOpenApi();

        app.MigrateDatabase();

        app.Run();
    }
}
