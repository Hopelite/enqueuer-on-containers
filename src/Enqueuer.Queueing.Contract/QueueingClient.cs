using Enqueuer.Queueing.Contract.V1.Commands;
using Enqueuer.Queueing.Contract.V1.Exceptions;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Enqueuer.Queueing.Contract.V1
{
    public class QueueingClient : IQueueingClient, IDisposable
    {
        private readonly HttpClient _httpClient;

        public QueueingClient(Uri queueingApiUrl)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = queueingApiUrl ?? throw new ArgumentNullException(nameof(queueingApiUrl))
            };
        }

        public async Task CreateQueueAsync(long groupId, string queueName, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PutAsync($"/api/groups/{groupId}/queues/{queueName}", content: null, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var reasonMessage = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new QueueAlreadyExistsException(reasonMessage);
                }

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new InvalidQueueNameException(reasonMessage);
                }

                throw new QueueingClientException($"Response code for queue creation indicates failure. Reason: {response.StatusCode}, {reasonMessage}");
            }
        }

        public async Task DeleteQueueAsync(long groupId, string queueName, CancellationToken cancellationToken)
        {
            var response = await _httpClient.DeleteAsync($"/api/groups/{groupId}/queues/{queueName}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var reasonMessage = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new QueueDoesNotExistException(reasonMessage);
                }

                throw new QueueingClientException("Response code for queue deletion indicates failure. Reason: {reasonMessage}");
            }
        }

        public async Task EnqueueParticipant(long groupId, string queueName, EnqueueParticipantCommand command, CancellationToken cancellationToken)
        {
            var content = JsonContent.Create(command);
            var response = await _httpClient.PostAsync($"/api/groups/{groupId}/queues/{queueName}/participants", content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var reasonMessage = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new QueueDoesNotExistException(reasonMessage);
                }

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new ParticipantAlreadyExistsException(reasonMessage);
                }

                throw new QueueingClientException("Response code for participant enqueueing indicates failure. Reason: {reasonMessage}");
            }
        }

        public async Task EnqueueParticipantAt(long groupId, string queueName, uint position, EnqueueParticipantAtCommand command, CancellationToken cancellationToken)
        {
            var content = JsonContent.Create(command);
            var response = await _httpClient.PutAsync($"/api/groups/{groupId}/queues/{queueName}/participants/{position}", content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var reasonMessage = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new QueueDoesNotExistException(reasonMessage);
                }

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    if (reasonMessage.Contains("already exists"))
                    {
                        throw new ParticipantAlreadyExistsException(reasonMessage);
                    }

                    if (reasonMessage.Contains("reserved position"))
                    {
                        throw new PositionReservedException(reasonMessage);
                    }
                }

                throw new QueueingClientException("Response code for participant enqueueing indicates failure. Reason: {reasonMessage}");
            }
        }

        public async Task DequeueParticipant(long groupId, string queueName, DequeueParticipantCommand command, CancellationToken cancellationToken)
        {
            var content = JsonContent.Create(command);
            var message = new HttpRequestMessage()
            {
                Content = content,
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"/api/groups/{groupId}/queues/{queueName}/participants", UriKind.Relative)
            };

            var response = await _httpClient.SendAsync(message, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var reasonMessage = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    if (reasonMessage.Contains("does not exist in the group"))
                    {
                        throw new QueueDoesNotExistException(reasonMessage);
                    }

                    if (reasonMessage.Contains("does not exist in the queue"))
                    {
                        throw new ParticipantDoesNotExistException(reasonMessage);
                    }
                }

                throw new QueueingClientException("Response code for participant dequeueing indicates failure. Reason: {reasonMessage}");
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
