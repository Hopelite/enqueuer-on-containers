using System;

namespace Enqueuer.Telegram.Notifications.Contract.V1.Configuration
{
    public class ChatConfigurationClientOptions
    {
        private Uri _baseAddress = null!;

        public Uri BaseAddress
        {
            get => _baseAddress;
            set => _baseAddress = value ?? throw new ArgumentNullException(nameof(BaseAddress));
        }
    }
}
