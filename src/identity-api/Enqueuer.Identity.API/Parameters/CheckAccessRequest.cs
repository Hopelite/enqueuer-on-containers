using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Identity.API.Parameters;

public class CheckAccessRequest : ResourceAuthorizationRequest
{
    [FromQuery(Name = "scope")]
    public string Scope { get; set; } = null!;
}
