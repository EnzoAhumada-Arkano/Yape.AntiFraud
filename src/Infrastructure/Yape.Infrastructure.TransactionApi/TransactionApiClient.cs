using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;
using Yape.Domain.ApiClient;

namespace Yape.Infrastructure.TransactionApi
{
    public class TransactionApiClient : ITransactionApiClient
    {
        private readonly ILogger<TransactionApiClient> _logger;
        private readonly HttpClient _httpClient;

        public TransactionApiClient(ILogger<TransactionApiClient> logger, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = new HttpClient() { BaseAddress = new Uri(configuration["TransactionApi:BaseAddress"]) };
        }
        public async Task UpdateStatusAsync(Guid transactionExternalId, int status)
        {
            _logger.LogInformation("TransactionApiClient:UpdateStatus - Call");
            var transaction = new
            {
                TransactionExternalId = transactionExternalId,
                Status = status
            };
            string json = JsonSerializer.Serialize(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.PutAsync($"SetStatus/ExternalId/{transactionExternalId}", content);
                httpResponseMessage.EnsureSuccessStatusCode();
                string responseBody = await httpResponseMessage.Content.ReadAsStringAsync();
                _logger.LogInformation("Response from API: {ResponseBody}", responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message to API.");
            }
            throw new NotImplementedException();
        }
    }
}
