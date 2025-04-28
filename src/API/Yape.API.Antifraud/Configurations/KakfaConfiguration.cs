using Yape.Domain.Message;
using Yape.Infrastructure.Kafka.TransactionMessage;

namespace Yape.API.AntiFraud.Configurations
{
    public static class KafkaConfiguration
    {
        public static IServiceCollection AddKafkaConfiguration(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IMessageProducer, TransactionMessageProducer>();
            return serviceCollection;
        }
    }
}
