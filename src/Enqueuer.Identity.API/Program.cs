using Enqueuer.Identity.API.Extensions;
using Enqueuer.Identity.Authorization;
using Enqueuer.Identity.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Enqueuer.Identity.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
        });

        builder.Services.AddSingleton<IAuthorizationService, AuthorizationService>()
                        .AddDbContext<IdentityContext>(options =>
                            options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityDB")));

        builder.ConfigureOAuth();

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

        app.UseAuthorization();
        app.MapControllers();

        app.MigrateDatabase();

        app.Run();
    }
}
