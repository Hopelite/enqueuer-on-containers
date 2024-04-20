using Enqueuer.Queueing.API.Application.Messaging;
using Enqueuer.Queueing.API.Extensions;
using Enqueuer.Queueing.Domain.Factories;
using Enqueuer.Queueing.Domain.Models;
using Enqueuer.Queueing.Domain.Repositories;
using Enqueuer.Queueing.Infrastructure.Messaging;
using Enqueuer.Queueing.Infrastructure.Persistence.Repositories;
using Enqueuer.Queueing.Infrastructure.Persistence.Storage;
using Enqueuer.Queueing.Infrastructure.Persistence.Storage.Helpers;
using Enqueuer.Queueing.Infrastructure.Persistence.Storage.Writing;

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
            configuration.MapDomainEvent<Domain.Events.QueueCreatedEvent, Contract.V1.Events.QueueCreatedEvent>();
            configuration.MapDomainEvent<Domain.Events.QueueDeletedEvent, Contract.V1.Events.QueueDeletedEvent>();



            //configuration.MapRejectedDomainEvent<Infrastructure.Messaging.RejectedEvent, Contract.V1.Events.RejectedEvents.QueueAlreadyExistsEvent>();
        });

        builder.Services.AddRabbitMQClient();

        // Event sourcing
        builder.Services.AddTransient<IGroupRepository, GroupRepository>();
        builder.Services.AddSingleton<IEventWriterManager<Group>, EventWriterManager>();
        builder.Services.AddTransient<IEventWriterFactory<Group>, GroupEventWriterFactory>();
        builder.Services.AddTransient<IGroupFactory, GroupFactory>();
        builder.Services.AddTransient<IAggregateRootBuilder<Group>, GroupAggregateBuilder>();
        builder.Services.AddSingleton<IEventStorage, DocumentEventStorage>();
        builder.Services.Configure<EventsDatabaseSettings>(builder.Configuration.GetSection("EventsDatabase"));
    }
}
