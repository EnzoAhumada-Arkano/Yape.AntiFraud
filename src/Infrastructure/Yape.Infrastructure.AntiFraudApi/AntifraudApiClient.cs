using System.Text.Json;
using System.Text;
using Yape.Domain.ApiClient;
using Microsoft.Extensions.Logging;

namespace Yape.Infrastructure.AntiFraudApi
{
    public class AntifraudApiClient : IAntifraudApiClient
    {
        private readonly ILogger<AntifraudApiClient> _logger;

        public AntifraudApiClient(ILogger<AntifraudApiClient> logger)
        {
            _logger = logger;
        }
        public async Task ValidateTransactionAsync(Guid transactionExternalId)
        {
            //Move to service api client
            HttpClient httpClient = new HttpClient() { BaseAddress = new Uri("http://localhost:5132/api/") };

            //create body
            var transaction = new
            {
                TransactionExternalId = transactionExternalId
            };
            string json = JsonSerializer.Serialize(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("AntiFraud/Validate", content);
                httpResponseMessage.EnsureSuccessStatusCode();
                string responseBody = await httpResponseMessage.Content.ReadAsStringAsync();
                _logger.LogInformation("Response from API: {ResponseBody}", responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message to API.");
            }
        }
    }
}
