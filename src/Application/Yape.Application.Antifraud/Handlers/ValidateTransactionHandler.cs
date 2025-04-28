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
            bool isValid = false;
            string message = string.Empty;
            _logger.LogInformation("ValidateTransactionHandler:Handle - Logic");
            var transaction = await _transactionRepository.GetTransactionsByExternalIdAsync(request.TransactionExternalId);

            //Validate value of the transaction
            if (transaction != null)
            {

                bool valueIsValid = ValidateValueTransaction(transaction);
                bool valueSumTodayIsValid = await ValidateValueSumTodaySourceAccountAsync(transaction);

                if (valueIsValid && valueSumTodayIsValid)
                {
                    _logger.LogInformation("Transaction is valid");
                    message = "Transaction is valid";
                    isValid = true;
                }
                else
                {
                    _logger.LogWarning("Transaction is invalid");
                    message = "Transaction is invalid";
                    isValid = false;
                }

               await _messageProducer.ProduceMessageAsync(new KafkaMessage
                {
                    Key = transaction.TransactionExternalId.ToString(),
                    Value = message
                }, cancellationToken);
            } 
            else
            {
                _logger.LogWarning("Transaction not found");
                message = "Transaction not found";
                isValid = false;
                await _messageProducer.ProduceMessageAsync(new KafkaMessage
                {
                    Key = request.TransactionExternalId.ToString(),
                    Value = message
                }, cancellationToken);
            }

            return isValid;
        }

        private bool ValidateValueTransaction(Domain.Entities.Transaction transaction)
        {
            if (transaction.Value > 2000)
            {
                _logger.LogWarning("Transaction value exceeds limit");
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateValueSumTodaySourceAccountAsync(Domain.Entities.Transaction transaction)
        {
            //Validate total transactions by user current day
            var transactions = await _transactionRepository.GetTransactionsSenderByAccountIdAsync(transaction.SourceAccountId);
            var transactionsToday = transactions.Where(x => x.CreatedAt.Date == DateTime.UtcNow.Date).ToList();
            //Sum the value of the transactions
            var totalValue = transactionsToday.Sum(x => x.Value);
            if (totalValue > 20000)
            {
                _logger.LogWarning("Total transaction value exceeds limit");
                return false;
            }
            return true;
        }
    }
}


