using System.Text.Json;
using Azure.Identity;
using Enqueuer.Identity.API.Extensions;
using Enqueuer.Identity.API.Services;
using Enqueuer.Identity.Authorization;
using Enqueuer.Identity.OAuth;
using Enqueuer.Identity.OAuth.Authorization;
using Enqueuer.Identity.OAuth.JWT;
using Enqueuer.Identity.OAuth.Storage;
using Enqueuer.Identity.OAuth.Tokens;
using Enqueuer.Identity.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

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

        builder.Services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
        });

        // RBAC
        builder.Services.AddSingleton<IAuthorizationService, AuthorizationService>()
                        .AddDbContext<IdentityContext>(options =>
                            options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityDB")));

        // OAuth
        builder.Services.AddMemoryCache();

        builder.Services.AddTransient<IOAuthService, OAuthService>()
                        .AddSingleton<IAccessTokenManager, AccessTokenManager>()
                        .AddSingleton<IAccessTokenStorage, InMemoryTokenStorage>()
                        .AddTransient<IAccessTokenGenerator, JwtTokenGenerator>()
                        .AddTransient<IGrantAuthorizerFactory, GrantAuthorizerFactory>()
                        .AddTransient<ISignatureProvider, InMemorySignatureProvider>()
                        .Configure<JwtTokenIssuingConfiguration>(builder.Configuration.GetRequiredSection("JwtTokenIssuing"))

                        .AddTransient<ClientCredentialsGrantAuthorizer>()
                        .AddSingleton<IClientCredentialsStorage, AzureKeyVaultStorage>()

                        .Configure<OAuthConfiguration>(builder.Configuration.GetRequiredSection("OAuth"));

        var keyVaultUri = builder.Configuration.GetRequiredSection("KeyVault")
                                               .GetRequiredUri("Url");

        builder.Services.AddAzureClients(azureBuilder =>
        {
            azureBuilder.AddSecretClient(keyVaultUri)
                .WithCredential(new DefaultAzureCredential());
        });

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtTokenAuthentication();

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
