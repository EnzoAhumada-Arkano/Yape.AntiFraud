using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Yape.Domain.ApiClient;
using Yape.Domain.Message;
using static Domain.Constants;

namespace Yape.Infrastructure.Kafka.TransactionMessage
{
    public class TransactionMessageConsumer : IMessageConsumer
    {
        private readonly string _topicName;
        private readonly string _bootstrapServers;
        private readonly string _groupId;
        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<TransactionMessageConsumer> _logger;
        private readonly ITransactionApiClient _transactionApiClient;

        public TransactionMessageConsumer(ILogger<TransactionMessageConsumer> logger, 
            IConfiguration configuration, 
            ITransactionApiClient transactionApiClient)
        {
            _topicName = configuration["Kafka:TransactionTopic"];
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
            _transactionApiClient = transactionApiClient;
        }

        public async Task CreateTopicIfNotExistsAsync()
        {
            _logger.LogInformation("TransactionMessageProducer:CreateTopicIfNotExistsAsync - Start");
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
                    _logger.LogError(ex, "An error occured creating topic");
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
                        Console.WriteLine($"Message consume from Kafka Key: '{cr.Message.Key}' - Value: '{cr.Message.Value}' at: '{cr.Topic}'.");
                        Guid transactionExternalId = Guid.Parse(cr.Message.Key);
                        if (cr.Message.Value.Contains(TrasanctionValidateStatus.TransactionValid))
                        {
                            await _transactionApiClient.UpdateStatusAsync(transactionExternalId, (int)TranstactionStatus.approved);
                        }

                        if (cr.Message.Value.Contains(TrasanctionValidateStatus.TransactionInvalid))
                        {
                            await _transactionApiClient.UpdateStatusAsync(transactionExternalId, (int)TranstactionStatus.rejected);
                        }
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
