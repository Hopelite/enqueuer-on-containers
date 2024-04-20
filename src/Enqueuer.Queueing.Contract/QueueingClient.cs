using Enqueuer.Queueing.Contract.V1.Commands;
using Enqueuer.Queueing.Contract.V1.Exceptions;
using System;
using System.Net;
using System.Net.Http;
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

                // TODO: consider handling 404 Code
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

        public Task EnqueueParticipant(long groupId, string queueName, EnqueueParticipantCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task EnqueueParticipantTo(long groupId, string queueName, uint position, EnqueueParticipantToCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
