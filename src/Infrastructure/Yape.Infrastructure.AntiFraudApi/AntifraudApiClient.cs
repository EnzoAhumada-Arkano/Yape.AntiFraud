using System.Text.Json;
using System.Text;
using Yape.Domain.ApiClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Yape.Infrastructure.AntiFraudApi
{
    public class AntifraudApiClient : IAntifraudApiClient
    {
        private readonly ILogger<AntifraudApiClient> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public AntifraudApiClient(ILogger<AntifraudApiClient> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = new HttpClient() { BaseAddress = new Uri(configuration["AntifraudApi:BaseAddress"]) };
        }
        public async Task ValidateTransactionAsync(Guid transactionExternalId)
        {
            //create body
            var transaction = new
            {
                TransactionExternalId = transactionExternalId
            };
            string json = JsonSerializer.Serialize(transaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.PostAsync("AntiFraud/Validate", content);
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
