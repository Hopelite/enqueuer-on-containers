using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Identity.API.Parameters;

public class CheckAccessQueryParameters
{
    [BindProperty(Name = "recource_uri")]
    public Uri RecourceId { get; set; } = null!;

    [BindProperty(Name = "user_id")]
    public long UserId { get; set; }

    [BindProperty(Name = "scope")]
    public string Scope { get; set; } = null!;
}
