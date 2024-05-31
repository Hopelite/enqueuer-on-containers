using System;

namespace Enqueuer.Telegram.Notifications.Contract.V1.Exceptions
{
    public class ChatConfigurationClientException : Exception
    {
        public ChatConfigurationClientException()
        {
        }

        public ChatConfigurationClientException(string message)
            : base(message)
        {
        }

        public ChatConfigurationClientException(string message, Exception innerExcepton)
            : base(message, innerExcepton)
        {
        }
    }
}
