using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Identity.API.Parameters;

public class GrantAccessRequest : ResourceAuthorizationRequest
{
    [FromQuery(Name = "role")]
    public string Role { get; set; } = null!;
}
