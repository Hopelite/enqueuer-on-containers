using Microsoft.AspNetCore.Mvc;

namespace Enqueuer.Identity.API.Parameters;

public class TelegramAuthorizationCallbackQueryParameters
{
    [BindProperty(Name = "id")]
    public long Id { get; set; }

    [BindProperty(Name = "first_name")]
    public string FirstName { get; set; } = null!;

    [BindProperty(Name = "username")]
    public string Username { get; set; } = null!;

    [BindProperty(Name = "hash")]
    public string Hash { get; set; } = null!;
}
