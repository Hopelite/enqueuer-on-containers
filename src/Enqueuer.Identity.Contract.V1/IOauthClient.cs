using Enqueuer.Identity.Contract.V1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Enqueuer.Identity.Contract.V1
{
    public interface IOauthClient
    {
        Task AuthorizeAsync(AuthorizationRequest request, CancellationToken cancellationToken);
    }
}
