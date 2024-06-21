using Enqueuer.Identity.Contract.V1;
using Enqueuer.Identity.Contract.V1.OAuth.RequestHandlers;
using Enqueuer.Telegram.BFF.Core.Configuration;
using Enqueuer.Telegram.BFF.Core.Factories;
using Enqueuer.Telegram.BFF.Core.Models.Callbacks;
using Enqueuer.Telegram.BFF.Core.Services;
using Enqueuer.Telegram.BFF.Localization;
using Enqueuer.Telegram.BFF.Messages;
using Enqueuer.Telegram.BFF.Messages.Factories;
using Enqueuer.Telegram.BFF.Services;
using Enqueuer.Telegram.BFF.Services.Caching;
using Enqueuer.Telegram.BFF.Services.Factories;
using Enqueuer.Telegram.Shared.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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

        builder.Services.AddTransient<IMessageContextFactory, MessageContextFactory>();
        builder.Services.AddTransient<IMessageHandlersFactory, MessageHandlersFactory>();
        builder.Services.AddScoped<IMessageDistributor, MessageDistributor>();
        builder.Services.AddMessageHandlers();
        builder.Services.AddSingleton<ILocalizationProvider, LocalizationProvider>();
        builder.AddTelegramClient();



        builder.Services.AddSingleton<IUserSynchronizationService, UserSynchronizationService>();
        builder.Services.AddSingleton<IUserInfoCache, InMemoryUserInfoCache>();



        builder.Services.Configure<IdentityClientOptions>(builder.Configuration.GetRequiredSection("IdentityProvider"), configure =>
                        {
                            configure.BindNonPublicProperties = true;
                        })
                        .AddTransient<ClientCredentialsTokenHandler<IIdentityClient>>()
                        .AddIdentityClient()
                        .AddHttpMessageHandler(serviceProvider =>
                        {
                            return serviceProvider.GetRequiredService<ClientCredentialsTokenHandler<IIdentityClient>>();
                        });

        builder.AddQueueingClient();

        builder.Services.Configure<ConfigurationCacheOptions>(builder.Configuration.GetRequiredSection("ConfigurationCacheOptions"));

        // TODO: verify this warning
        var cacheConfiguration = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<ConfigurationCacheOptions>>().Value;
        builder.Services.AddMemoryCache(options =>
        {
            options.SizeLimit = cacheConfiguration.MaxSize; // An arbitrary value - may be memory size or entry count
        });
        builder.Services.AddSingleton<IChatConfigurationCache, InMemoryGroupConfigurationCache>();
        builder.AddChatConfigurationClient();
        builder.Services.AddRabbitMQClient()
                        .SubscribeAllHandlers();

        var app = builder.Build();

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

        app.MapPost("/bot", async (Update telegramUpdate, [FromServices] IServiceProvider serviceProvider, CancellationToken cancellationToken) =>
        {
            if (telegramUpdate == null)
            {
                return Results.BadRequest();
            }

            if (telegramUpdate.Type == UpdateType.Message)
            {
                var messageContextFactory = serviceProvider.GetRequiredService<IMessageContextFactory>();
                var messageContext = await messageContextFactory.CreateMessageContextAsync(telegramUpdate.Message!, cancellationToken);
                if (messageContext == null)
                {
                    return Results.Ok();
                }

                var distributor = serviceProvider.GetRequiredService<IMessageDistributor>();
                await distributor.DistributeAsync(messageContext, cancellationToken);
            }
            else if (telegramUpdate.Type == UpdateType.CallbackQuery && CallbackContext.TryCreate(telegramUpdate.CallbackQuery!, out var callbackContext))
            {
                throw new NotImplementedException();
            }

            return Results.Ok();
        });

        app.Run();
    }
}
