using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
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

        public AntifraudMessageProducer(IConfiguration configuration)
        {

            var config = new ProducerConfig()
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };
            _topic = configuration["Kafka:Topic"];
            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task ProduceMessageAsync(KafkaMessage message)
        {
            var result = await _producer.ProduceAsync(_topic, new Message<string, string>
            {
                Key = message.Key,
                Value = message.Value
            });

            Console.WriteLine($"Message sent to Kafka: {result.TopicPartitionOffset}");
        }
    }
}
