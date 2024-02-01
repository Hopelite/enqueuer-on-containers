using Enqueuer.Telegram.Notifications.Models;
using Microsoft.AspNetCore.Mvc;

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
        builder.Services.AddHostedService<EventsListener>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapPut("/chats/{chatId}/language", (long chatId, [FromBody] ChatLanguage chatLanguage) =>
        {
            // TODO: add validation whether user has rights to change language via Identity API


            return Results.Ok();
        })
        .WithName("SetChatLanguage")
        .WithOpenApi();

        app.Run();
    }
}
