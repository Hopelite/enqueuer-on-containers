using Enqueuer.Identity.Authorization;
using Enqueuer.Identity.Authorization.Grants.Validation;
using Enqueuer.Identity.Authorization.Validation;
using Enqueuer.Identity.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace Enqueuer.Identity.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
        });

        builder.Services.AddSingleton<IOAuthService, OAuthService>();
        builder.Services.AddSingleton<IScopeValidator, ScopeValidator>();
        builder.Services.AddSingleton<IAuthorizationGrantValidator, AuthorizationGrantValidator>();

        builder.Services.AddSingleton<IAuthorizationService, AuthorizationService>();

        builder.Services.AddDbContext<IdentityContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityDB")));

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MYBIGGESTKEYPOSSIBLEJUSTLOOKATTHISDUDE")),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,  // Remove delay of token when expire
            };
        });

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
}
