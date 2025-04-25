using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Yape.Domain.ApiClient;
using Yape.Domain.Message;

namespace Yape.Infrastructure.Kafka.AntiFraudMessage
{
    public class AntifraudMessageConsumer : IMessageConsumer
    {
        private readonly string _topic;
        private readonly IConsumer<string, string> _consumer;
        private readonly IAntifraudApiClient _antifraudApiClient;

        public AntifraudMessageConsumer(IConfiguration configuration, IAntifraudApiClient antifraudApiClient)
        {
            var config = new ConsumerConfig
            {
                //BootstrapServers = configuration["Kafka:BootstrapServers"],
                BootstrapServers = "localhost:9092",
                GroupId = "dotnet-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            //_topic = configuration["Kafka:Topic"];
            _topic = "yape-antifraud-topic";
            _antifraudApiClient = antifraudApiClient;
        }
        public async Task ConsumeMessageAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe(_topic);

            await Task.Run(async () =>
            {                
                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var cr = _consumer.Consume(cancellationToken);
                        Console.WriteLine($"Message consume from Kafka Key: '{cr.Message.Key}' - Value: '{cr.Message.Value}' at: '{cr.Topic}'.");
                        Guid transactionExternalId = Guid.Parse(cr.Message.Key);
                        await _antifraudApiClient.ValidateTransactionAsync(transactionExternalId);
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
