using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Enqueuer.Identity.API.Parameters.Binders;

public class CreateOrUpdateUserRequestBinder(IOptions<JsonOptions> options) : IModelBinder
{
    private readonly JsonOptions _options = options.Value;

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (!bindingContext.ActionContext.RouteData.Values.TryGetValue(CreateOrUpdateUserRequest.UserIdRouteParameter, out var userIdValue) || userIdValue == null)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        if (!long.TryParse(userIdValue.ToString(), out var userId))
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        var request = bindingContext.HttpContext.Request;
        request.EnableBuffering();

        string? requestBody;
        using (var reader = new StreamReader(request.Body))
        {
            requestBody = reader.ReadToEndAsync().Result;
            request.Body.Position = 0;
        }

        var bodyData = JsonSerializer.Deserialize<CreateOrUpdateUserRequestBody>(requestBody, _options.JsonSerializerOptions);
        if (bodyData == null)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        var model = new CreateOrUpdateUserRequest
        {
            UserId = userId,
            FirstName = bodyData.FirstName,
            LastName = bodyData.LastName
        };

        bindingContext.Result = ModelBindingResult.Success(model);
        return Task.CompletedTask;
    }

    private record CreateOrUpdateUserRequestBody(string FirstName, string LastName);
}
