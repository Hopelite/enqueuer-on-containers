using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Enqueuer.Identity.OAuth.JWT;

public interface ISignatureProvider
{
    SigningCredentials GetSigningCredentials();
}
