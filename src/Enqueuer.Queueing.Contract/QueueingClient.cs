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
            try
            {
                var response = await _httpClient.PutAsync($"/api/groups/{groupId}/queues/{queueName}", content: null, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    var reasonMessage = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.Conflict)
                    {
                        throw new ResourceAlreadyExistsException(reasonMessage);
                    }

                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        throw new InvalidQueueNameException(reasonMessage);
                    }

                    throw new QueueingClientException($"Response code for queue creation indicates failure. Reason: {response.StatusCode}, {reasonMessage}");
                }
            }
            catch (Exception ex)
            {
                throw new QueueingClientException("An error occured during Queueing API request.", ex);
            }
        }

        public async Task DeleteQueueAsync(long groupId, string queueName, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/groups/{groupId}/queues/{queueName}", cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var reasonMessage = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new ResourceDoesNotExistException(reasonMessage);
                    }

                    throw new QueueingClientException("Response code for queue deletion indicates failure. Reason: {reasonMessage}");
                }
            }
            catch (Exception ex)
            {
                throw new QueueingClientException("An error occured during Queueing API request.", ex);
            }
        }

        public async Task EnqueueParticipant(long groupId, string queueName, EnqueueParticipantCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var content = JsonContent.Create(command);
                var response = await _httpClient.PostAsync($"/api/groups/{groupId}/queues/{queueName}/participants", content, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var reasonMessage = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new ResourceDoesNotExistException(reasonMessage);
                    }

                    if (response.StatusCode == HttpStatusCode.Conflict)
                    {
                        throw new ParticipantAlreadyExistsException(reasonMessage);
                    }

                    throw new QueueingClientException("Response code for queue deletion indicates failure. Reason: {reasonMessage}");
                }
            }
            catch (Exception ex)
            {
                throw new QueueingClientException("An error occured during Queueing API request.", ex);
            }
        }

        public async Task EnqueueParticipantAt(long groupId, string queueName, uint position, EnqueueParticipantAtCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var content = JsonContent.Create(command);
                var response = await _httpClient.PutAsync($"/api/groups/{groupId}/queues/{queueName}/participants/{position}", content, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var reasonMessage = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new ResourceDoesNotExistException(reasonMessage);
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

                    throw new QueueingClientException("Response code for queue deletion indicates failure. Reason: {reasonMessage}");
                }
            }
            catch (Exception ex)
            {
                throw new QueueingClientException("An error occured during Queueing API request.", ex);
            }
        }

        public Task DequeueParticipant(long groupId, string queueName, DequeueParticipantCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
