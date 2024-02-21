using Enqueuer.Queueing.Contract.V1.Commands;
using Enqueuer.Queueing.Contract.V1.Commands.ViewModels;
using Enqueuer.Queueing.Contract.V1.Exceptions;
using System;
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

        public async Task CreateQueueAsync(CreateQueueCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var content = JsonContent.Create(command);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync("/api/queues/", content, cancellationToken);
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

        public Task DeleteGroupQueue(DeleteGroupQueueCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<EnqueuedParticipantViewModel> EnqueueParticipant(int queueId, EnqueueParticipantCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
