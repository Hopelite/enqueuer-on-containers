using Enqueuer.Queueing.API.Extensions;
using Enqueuer.Queueing.Domain.Factories;
using Enqueuer.Queueing.Domain.Repositories;
using Enqueuer.Queueing.Infrastructure.Persistence;
using Enqueuer.Queueing.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

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

        app.UseHttpsRedirection();

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
        builder.Services.AddDbContext<QueueingContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("QueueingDB"));
        });

        builder.Services.AddScoped<IQueueRepository, QueueRepository>();
        builder.Services.AddTransient<IQueueFactory, QueueFactory>();

        builder.Services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });

        builder.Services.MigrateDatabase();
    }
}
