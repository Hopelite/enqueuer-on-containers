using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enqueuer.Identity.OAuth.Tokens;

public interface IAccessTokenGenerator
{
    AccessToken GenerateToken(AccessTokenContext context);
}
