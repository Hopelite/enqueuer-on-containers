using Enqueuer.Identity.OAuth.Models;
using Enqueuer.Identity.OAuth.Models.Grants;
using Enqueuer.OAuth.Core.Tokens;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Enqueuer.Identity.API.Parameters.Binders;

public class AuthorizationGrantModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var grantTypeValue = bindingContext.ValueProvider.GetValue("grant_type").FirstValue;
        IAuthorizationGrant? grant;

#pragma warning disable CS8604
        try
        {
            var scope = new Scope(bindingContext.ValueProvider.GetValue("scope").FirstValue);
            grant = grantTypeValue switch
            {
                AuthorizationGrantType.ClientCredentials.Type => new ClientCredentialsGrant(
                    bindingContext.ValueProvider.GetValue("client_id").FirstValue,
                    bindingContext.ValueProvider.GetValue("client_secret").FirstValue,
                    scope),
                AuthorizationGrantType.AuthorizationCode.Type => new AuthorizationCodeGrant(
                    bindingContext.ValueProvider.GetValue("code").FirstValue,
                    new Uri(bindingContext.ValueProvider.GetValue("redirect_uri").FirstValue, UriKind.Relative),
                    bindingContext.ValueProvider.GetValue("client_id").FirstValue),
                _ => null
            };
        }
        catch (Exception)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }
#pragma warning restore CS8604

        bindingContext.Result = grant == null
            ? ModelBindingResult.Failed()
            : ModelBindingResult.Success(grant);

        return Task.CompletedTask;
    }
}
