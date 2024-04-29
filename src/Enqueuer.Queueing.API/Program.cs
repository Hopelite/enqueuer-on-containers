using Enqueuer.Queueing.API.Application.Messaging;
using Enqueuer.Queueing.API.Authorization.Requirements;
using Enqueuer.Queueing.API.Mapping;
using Enqueuer.Queueing.API.Mapping.RejectedEvents;
using Enqueuer.Queueing.Domain.Factories;
using Enqueuer.Queueing.Domain.Models;
using Enqueuer.Queueing.Domain.Repositories;
using Enqueuer.Queueing.Infrastructure.Commands.Handling;
using Enqueuer.Queueing.Infrastructure.Messaging;
using Enqueuer.Queueing.Infrastructure.Persistence.Repositories;
using Enqueuer.Queueing.Infrastructure.Persistence.Storage;
using Enqueuer.Queueing.Infrastructure.Persistence.Storage.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Enqueuer.Queueing.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // TODO: enable once add certificates
        //app.UseHttpsRedirection();

        // TODO: add correlationId

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = "https://localhost:7279";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MYBIGGESTKEYPOSSIBLEJUSTLOOKATTHISDUDE")),
                ValidAudience = "apis",
                ValidIssuer = "EnqueuerIdentity",
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("QueueDeletionPolicy", policy =>
            {
                policy.Requirements.Clear();
                policy.Requirements.Add(new QueueDeleteRequirement());
            });
        });

        builder.Services.AddSingleton<IAuthorizationHandler, QueueDeleteHandler>();
        builder.Services.AddHttpContextAccessor();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddTransient<IEventPublisher, BusEventPublisher>();

        builder.Services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });

        builder.Services.AddAutoMapper(configuration =>
        {
            configuration.AddProfile<QueueCreatedEventMapProfile>();
            configuration.AddProfile<QueueDeletedEventMapProfile>();
            configuration.AddProfile<ParticipantEnqueuedEventMapProfile>();
            configuration.AddProfile<ParticipantEnqueuedAtEventMapProfile>();
            configuration.AddProfile<ParticipantDequeuedEventMapProfile>();

            configuration.AddProfile<RejectedCommandMapProfile>();
        });

        builder.Services.AddRabbitMQClient();

        // Event sourcing
        builder.Services.AddTransient<IGroupRepository, GroupRepository>();
        builder.Services.AddTransient<IGroupFactory, GroupFactory>();
        builder.Services.AddTransient<IAggregateRootBuilder<Group>, GroupAggregateBuilder>();
        builder.Services.AddSingleton<IEventStorage, DocumentEventStorage>();
        builder.Services.Configure<EventsDatabaseSettings>(builder.Configuration.GetSection("EventsDatabase"));
        builder.Services.AddSingleton<ICommandHandlerManager<Group>, CommandHandlerManager>();
        builder.Services.AddTransient<ICommandHandlerFactory<Group>, GroupCommandHandlerFactory>();
    }
}
