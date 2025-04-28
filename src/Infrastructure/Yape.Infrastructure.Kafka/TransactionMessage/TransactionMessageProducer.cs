using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.Domain.Entities;
using Yape.Domain.Message;
using Yape.Infrastructure.Kafka.AntiFraudMessage;

namespace Yape.Infrastructure.Kafka.TransactionMessage
{
    public class TransactionMessageProducer : IMessageProducer
    {
        private readonly string _topicName;
        private readonly string _bootstrapServers;
        private readonly IProducer<string, string> _producer;
        private readonly ILogger<TransactionMessageProducer> _logger;

        public TransactionMessageProducer(ILogger<TransactionMessageProducer> logger, IConfiguration configuration)
        {
            _topicName = configuration["Kafka:TransactionTopic"];
            _bootstrapServers = configuration["Kafka:BootstrapServers"];

            var config = new ProducerConfig()
            {
                BootstrapServers = _bootstrapServers,
                AllowAutoCreateTopics = true,
                Acks = Acks.All,
            };
            _producer = new ProducerBuilder<string, string>(config).Build();
            _logger = logger;
        }

        public async Task ProduceMessageAsync(KafkaMessage message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("TransactionMessageProducer:ProduceMessageAsync - Start");
            var pr = await _producer.ProduceAsync(topic: _topicName,
            new Message<string, string>
            {
                Key = message.Key,
                Value = message.Value
            }, cancellationToken);
            Console.WriteLine($"Message sent to Kafka Key: '{pr.Message.Key}' - Value: '{pr.Message.Value}' at: '{pr.Topic}'.");
        }
    }
}
