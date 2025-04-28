using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly string _topicName;
        private readonly string _bootstrapServers;
        private readonly string _groupId;
        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<AntifraudMessageConsumer> _logger;
        private readonly IAntifraudApiClient _antifraudApiClient;

        public AntifraudMessageConsumer(ILogger<AntifraudMessageConsumer> logger, IConfiguration configuration, IAntifraudApiClient antifraudApiClient)
        {
            _topicName = configuration["Kafka:AntifraudTopic"];
            _bootstrapServers = configuration["Kafka:BootstrapServers"];
            _groupId = configuration["Kafka:GroupId"];

            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = _groupId,
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _logger = logger;
            _antifraudApiClient = antifraudApiClient;
        }

        public async Task CreateTopicIfNotExistsAsync()
        {
            _logger.LogInformation("AntifraudMessageConsumer:CreateTopicIfNotExistsAsync - Start");
            var config = new AdminClientConfig { BootstrapServers = _bootstrapServers };

            using (var adminClient = new AdminClientBuilder(config).Build())
            {
                try
                {
                    var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
                    var topicExists = metadata.Topics.Any(t => t.Topic == _topicName);

                    if (!topicExists)
                    {
                        await adminClient.CreateTopicsAsync(new TopicSpecification[]
                        {
                        new TopicSpecification
                        {
                            Name = _topicName,
                            NumPartitions = 3,
                            ReplicationFactor = 1
                        }
                        });

                        _logger.LogInformation("Topic '{TopicName}' created.", _topicName);
                    }
                    else
                    {
                        _logger.LogInformation("Topic '{TopicName}' already exists.", _topicName);
                    }
                }
                catch (CreateTopicsException ex)
                {
                    _logger.LogError(ex, "An error occured creating topic - {Message}", ex.Message);
                }
            }
        }

        public async Task ConsumeMessageAsync(CancellationToken cancellationToken)
        {
            await CreateTopicIfNotExistsAsync();
            _consumer.Subscribe(_topicName);

            await Task.Run(async () =>
            {                
                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var cr = _consumer.Consume(cancellationToken);
                        _logger.LogInformation("Message consume from Kafka Key: '{Key}' - Value: '{Value}' at: '{Topic}'.", cr.Message.Key, cr.Message.Value, cr.Topic );
                        Guid transactionExternalId = Guid.Parse(cr.Message.Key);
                        //Call the antifraud API to validate the transaction
                        await _antifraudApiClient.ValidateTransactionAsync(transactionExternalId);
                    }
                }
                catch (OperationCanceledException)
                {
                    _consumer.Close();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error consuming message: {Message}", ex.Message);
                }
            }, cancellationToken);

        }
    }
}
