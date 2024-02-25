using Enqueuer.Queueing.Contract.V1.Exceptions;
using System;
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
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync($"/api/groups/{groupId}/queues/{queueName}", content: null, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    throw new QueueingClientException("Response code for queue creation indicates failure.");
                }
            }
            catch (Exception ex)
            {
                throw new QueueingClientException("An error occured during Queueing API request.", ex);
            }
        }

        public Task DeleteGroupQueue(long groupId, string queueName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
