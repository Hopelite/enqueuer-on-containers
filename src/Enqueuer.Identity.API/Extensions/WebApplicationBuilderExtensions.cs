using Azure.Identity;
using Enqueuer.Identity.API.Services;
using Enqueuer.Identity.Authorization;
using Enqueuer.Identity.Authorization.Grants;
using Enqueuer.Identity.Authorization.Grants.Credentials;
using Enqueuer.Identity.Authorization.Grants.Validation;
using Enqueuer.Identity.Authorization.OAuth;
using Enqueuer.Identity.Authorization.OAuth.Signature;
using Enqueuer.Identity.Authorization.Validation;
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
                        .AddTransient<IAuthorizationGrantValidator, AuthorizationGrantValidator>()
                        .AddTransient<IAuthorizationContext, AuthorizationContext>()
                        .AddTransient<IScopeValidator, ScopeValidator>()
                        .AddSingleton<IClientCredentialsStorage, AzureKeyVaultStorage>()
                        .AddTransient<ISignatureProviderFactory, SignatureProviderFactory>()
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
