using Yape.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using Yape.Application.Antifraud.Commands;
using static Domain.Constants;
using Yape.Domain.Message;
using Yape.Domain.Entities;

namespace Yape.Application.Antifraud.Handlers
{
    public class ValidateTransactionHandler : IRequestHandler<ValidateTransactionCommand, bool>
    {
        private readonly ILogger<ValidateTransactionHandler> _logger;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMessageProducer _messageProducer;

        public ValidateTransactionHandler(ILogger<ValidateTransactionHandler> logger, ITransactionRepository transactionRepository, IMessageProducer messageProducer)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
            _messageProducer = messageProducer;
        }
        public async Task<bool> Handle(ValidateTransactionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ValidateTransactionHandler:Handle - Logic");
            var transaction = await _transactionRepository.GetTransactionsByExternalIdAsync(request.TransactionExternalId);

            if (transaction == null)
            {
                _logger.LogWarning("Transaction not found - {Transaction}", request.TransactionExternalId);
                await ProduceMessageAsync(request.TransactionExternalId, "Transaction not found", cancellationToken);
                return false;
            }

            bool valueIsValid = ValidateValueTransaction(transaction);
            bool valueSumTodayIsValid = await ValidateValueSumTodaySourceAccountAsync(transaction);

            string message = valueIsValid && valueSumTodayIsValid
                ? TrasanctionValidateStatus.TransactionValid
                : TrasanctionValidateStatus.TransactionInvalid;

            _logger.LogInformation("{Message} - {Transaction}", message, transaction.TransactionExternalId);
            await ProduceMessageAsync(transaction.TransactionExternalId, message, cancellationToken);

            return valueIsValid && valueSumTodayIsValid;
        }

        private async Task ProduceMessageAsync(Guid transactionId, string message, CancellationToken cancellationToken)
        {
            await _messageProducer.ProduceMessageAsync(new KafkaMessage
            {
                Key = transactionId.ToString(),
                Value = message
            }, cancellationToken);
        }

        private bool ValidateValueTransaction(Transaction transaction)
        {
            if (transaction.Value > 2000)
            {
                _logger.LogWarning("Transaction value exceeds limit - {Transaction}", transaction.TransactionExternalId);
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateValueSumTodaySourceAccountAsync(Transaction transaction)
        {
            //Validate total transactions by user current day
            var transactions = await _transactionRepository.GetTransactionsSenderByAccountIdAsync(transaction.SourceAccountId);
            var transactionsToday = transactions.Where(x => x.CreatedAt.Date == DateTime.UtcNow.Date).ToList();
            //Sum the value of the transactions
            var totalValue = transactionsToday.Sum(x => x.Value);
            if (totalValue > 20000)
            {
                _logger.LogWarning("Total transaction value exceeds limit - Account: {SourceAccount}", transaction.SourceAccountId);
                return false;
            }
            return true;
        }
    }
}


