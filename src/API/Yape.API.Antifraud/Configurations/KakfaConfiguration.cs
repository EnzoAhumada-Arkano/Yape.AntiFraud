using Yape.Domain.Message;
using Yape.Infrastructure.Kafka.TransactionMessage;

namespace Yape.API.Antifraud.Configurations
{
    public static class KafkaConfiguration
    {
        public static IServiceCollection ConfigureKafka(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IMessageProducer, TransactionMessageProducer>();
            return serviceCollection;
        }
    }
}
