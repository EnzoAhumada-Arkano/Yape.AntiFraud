using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.Domain.Entities;
using Yape.Domain.Message;

namespace Yape.Infrastructure.Kafka.AntiFraudMessage
{
    public class AntifraudMessageProducer : IMessageProducer
    {
        private readonly string _topic;
        private readonly IProducer<string, string> _producer;

        public AntifraudMessageProducer(ILogger<AntifraudMessageProducer> logger, IConfiguration configuration)
        {

            var config = new ProducerConfig()
            {
                //BootstrapServers = configuration["Kafka:BootstrapServers"]
                BootstrapServers = "localhost:9092",
                AllowAutoCreateTopics = true,
                Acks = Acks.All,
            };
            //_topic = configuration["Kafka:Topic"];
            _topic = "yape-antifraud-topic";
            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task ProduceMessageAsync(KafkaMessage message, CancellationToken cancellationToken)
        {
            var result = await _producer.ProduceAsync(topic: _topic, 
            new Message<string, string>
            {
                Key = message.Key,
                Value = message.Value
            }, cancellationToken);

            Console.WriteLine($"Message sent to Kafka: {result.TopicPartitionOffset}");
        }
    }
}
