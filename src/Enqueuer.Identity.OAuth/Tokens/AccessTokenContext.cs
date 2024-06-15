using Enqueuer.Identity.OAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enqueuer.Identity.OAuth.Tokens;

public class AccessTokenContext
{
    public AccessTokenContext(string audience, Scope scope)
    {
        Audience = audience;
        Scope = scope;
    }

    public string Audience { get; }

    public Scope Scope { get; }
}
