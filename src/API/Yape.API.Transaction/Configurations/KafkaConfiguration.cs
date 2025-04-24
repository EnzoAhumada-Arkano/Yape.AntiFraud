using Yape.Domain.Message;
using Yape.Infrastructure.Kafka.AntiFraudMessage;

namespace Yape.API.Transaction.Configurations
{
    public static class KafkaConfiguration
    {
        public static IServiceCollection ConfigureKafkaProducer(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IMessageProducer, AntifraudMessageProducer>();
            return serviceCollection;
        }
    }
}
