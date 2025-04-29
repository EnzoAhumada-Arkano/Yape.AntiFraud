using Yape.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using Yape.Domain.Message;
using Yape.Domain.Entities;
using Yape.Application.Transaction.Commands;


namespace Yape.Application.Transaction.Handlers
{
    public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, Guid>
    {
        private readonly ILogger<CreateTransactionHandler> _logger;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMessageProducer _messageProducer;

        public CreateTransactionHandler(
            ILogger<CreateTransactionHandler> logger, 
            ITransactionRepository transactionRepository, 
            IMessageProducer messageProducer)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
            _messageProducer = messageProducer;
        }

        public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CreateTransactionHandler:Handle - Logic");
            // Validate the command
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // Map command to entity
            var transaction = new Domain.Entities.Transaction
            {
                SourceAccountId = request.SourceAccountId,
                TargetAccountId = request.TargetAccountId,
                TransferTypeId = request.TransferTypeId,
                Value = request.Value,
            };
            // Save to database
            await _transactionRepository.AddAsync(transaction);
            _logger.LogInformation("CreateTransactionHandler:Handle - Created transation {Transaction} into database", transaction.TransactionExternalId);
            // Send message to Kafka
            _logger.LogInformation("CreateTransactionHandler:Handle - Send message to kafka transaction {Transaction} created ", transaction.TransactionExternalId);
            await ProduceMessageAsync(transaction.TransactionExternalId, "Transaction created", cancellationToken);

            return transaction.TransactionExternalId;
        }

        private async Task ProduceMessageAsync(Guid transactionId, string message, CancellationToken cancellationToken)
        {
            await _messageProducer.ProduceMessageAsync(new QueueMessage
            {
                Key = transactionId.ToString(),
                Value = message
            }, cancellationToken);
        }
    }
}
