using Yape.Domain.ApiClient;
using Yape.Domain.Message;
using Yape.Infrastructure.AntiFraudApi;
using Yape.Infrastructure.Kafka.AntiFraudMessage;

namespace Yape.AntiFraud.Worker.Configurations
{
    public static class ServiceConfiguration
    { 
        public static IServiceCollection AddServiceConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IMessageConsumer, AntifraudMessageConsumer>();
            services.AddScoped<IAntifraudApiClient, AntifraudApiClient>();
            return services;
        }
    }
}
