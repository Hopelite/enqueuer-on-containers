using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enqueuer.Telegram.Shared.Types;

// TODO: consider better model for callbacks instead of hardcoded properties
// Maybe move to API Contracts
public class CallbackData
{
    public string Command { get; set; }

    public int? QueueId { get; set; }
}
