using Enqueuer.Telegram.BFF.Core.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enqueuer.Telegram.BFF.Messages.Handlers;

public class CreateQueueMessageHandler : IMessageHandler
{
    public Task HandleAsync(MessageContext messageContext, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
