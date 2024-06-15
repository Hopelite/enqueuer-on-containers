using Azure.Identity;
using Enqueuer.Identity.API.Services;
using Enqueuer.Identity.Authorization.Validation;
using Enqueuer.Identity.OAuth;
using Enqueuer.Identity.OAuth.JWT;
using Enqueuer.Identity.OAuth.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Azure;

namespace Enqueuer.Identity.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureOAuth(this WebApplicationBuilder builder)
    {
        var keyVaultUri = builder.Configuration.GetRequiredSection("KeyVault")
                                               .GetRequiredUri("Url");

        builder.Services.AddAzureClients(azureBuilder =>
        {
            azureBuilder.AddSecretClient(keyVaultUri)
                .WithCredential(new DefaultAzureCredential());
        });

        builder.Services.AddTransient<IOAuthService, OAuthService>()
                        .AddTransient<IScopeValidator, ScopeValidator>()
                        .AddSingleton<IClientCredentialsStorage, AzureKeyVaultStorage>()
                        .AddSingleton<ISignatureProvider, InMemorySignatureProvider>()
                        //.AddTransient<ISignatureProviderFactory, SignatureProviderFactory>()
                        .Configure<OAuthConfiguration>(builder.Configuration.GetRequiredSection("OAuth"));

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtTokenAuthentication();

        return builder;
    }
}
