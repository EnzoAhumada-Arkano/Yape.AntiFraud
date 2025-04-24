using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.Domain.Message;

namespace Yape.Infrastructure.Kafka.AntiFraudMessage
{
    public class AntifraudMessageConsumer : IMessageConsumer
    {
        private readonly string _topic;
        private readonly IConsumer<string, string> _consumer;

        public AntifraudMessageConsumer(IConfiguration configuration)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = "dotnet-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _topic = configuration["Kafka:Topic"]!;
        }
        public async Task ConsumeMessageAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe(_topic);

            await Task.Run(() =>
            {                
                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var cr = _consumer.Consume(cancellationToken);
                        Console.WriteLine($"Consumed message '{cr.Message}' at: '{cr.TopicPartitionOffset}'.");
                    }
                }
                catch (OperationCanceledException)
                {
                    _consumer.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error consuming message: {ex.Message}");
                }
            }, cancellationToken);

        }
    }
}
