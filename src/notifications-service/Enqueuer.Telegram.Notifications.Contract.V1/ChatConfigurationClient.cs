using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Enqueuer.Telegram.Notifications.Contract.V1.Exceptions;
using Enqueuer.Telegram.Notifications.Contract.V1.Models;

namespace Enqueuer.Telegram.Notifications.Contract.V1
{
    public class ChatConfigurationClient : IChatConfigurationClient
    {
        private readonly HttpClient _httpClient;

        public ChatConfigurationClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ChatNotificationsConfiguration> GetNewOrExistingConfigurationAsync(long chatId, string? languageCode, CancellationToken cancellationToken)
        {
            var uri = string.IsNullOrWhiteSpace(languageCode)
                ? new Uri($"chats/{chatId}", UriKind.Relative)
                : new Uri($"chats/{chatId}?language_code={languageCode}", UriKind.Relative);

            var response = await _httpClient.GetAsync(uri, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var reasonMessage = await response.Content.ReadAsStringAsync();
                throw new ChatConfigurationClientException($"Response code for chat '{chatId}' configuration retrieval indicates failure. Reason: {response.StatusCode}, {reasonMessage}");
            }

            var configuration = await response.Content.ReadFromJsonAsync<ChatNotificationsConfiguration>(cancellationToken);
            if (configuration == null)
            {
                throw new ChatConfigurationClientException("Unable to deserialize response into chat configuration.");
            }

            return configuration;
        }

        public async Task ConfigureChatNotificationsAsync(ChatNotificationsConfiguration chatConfiguration, CancellationToken cancellationToken)
        {
            if (chatConfiguration == null)
            {
                throw new ArgumentNullException(nameof(chatConfiguration));
            }

            var content = JsonContent.Create(chatConfiguration);
            var response = await _httpClient.PutAsync($"/chats/{chatConfiguration.ChatId}", content, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var reasonMessage = await response.Content.ReadAsStringAsync();
                throw new ChatConfigurationClientException($"Response code for chat '{chatConfiguration.ChatId}' configuration indicates failure. Reason: {response.StatusCode}, {reasonMessage}");
            }
        }
    }
}
