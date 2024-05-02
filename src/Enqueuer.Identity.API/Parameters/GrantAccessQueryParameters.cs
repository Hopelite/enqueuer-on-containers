using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Identity.API.Parameters;

public class GrantAccessQueryParameters
{
    private Uri _recourceId = null!;

    [FromRoute(Name = "resource_id")]
    public Uri RecourceId
    {
        get => _recourceId;
        set
        {
            var unescapedUri = Uri.UnescapeDataString(value.ToString());
            _recourceId = new Uri(unescapedUri, UriKind.Relative);
        }
    }

    [FromQuery(Name = "user_id")]
    public long UserId { get; set; }

    [FromQuery(Name = "role")]
    public string Role { get; set; } = null!;
}
