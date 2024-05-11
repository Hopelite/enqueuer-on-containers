using Enqueuer.Identity.Authorization.Grants;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Enqueuer.Identity.API.Parameters;

public class AuthorizationGrantModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var grantTypeValue = bindingContext.ValueProvider.GetValue("grant_type").FirstValue;
        IAuthorizationGrant? grant;

#pragma warning disable CS8604
        try
        {
            grant = grantTypeValue switch
            {
                AuthorizationGrantType.ClientCredentials => new ClientCredentialsGrant(
                    bindingContext.ValueProvider.GetValue("client_id").FirstValue,
                    bindingContext.ValueProvider.GetValue("client_secret").FirstValue),
                AuthorizationGrantType.AuthorizationCode => new AuthorizationCodeGrant(
                    bindingContext.ValueProvider.GetValue("code").FirstValue,
                    bindingContext.ValueProvider.GetValue("redirect_uri").FirstValue,
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
