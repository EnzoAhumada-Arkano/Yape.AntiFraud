using Yape.Domain.ApiClient;
using Yape.Domain.Message;
using Yape.Infrastructure.Kafka.TransactionMessage;
using Yape.Infrastructure.TransactionApi;

namespace Yape.Transaction.Worker.Configurations
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddServiceConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IMessageConsumer, TransactionMessageConsumer>();
            services.AddScoped<ITransactionApiClient, TransactionApiClient>();
            return services;
        }
    }
}
