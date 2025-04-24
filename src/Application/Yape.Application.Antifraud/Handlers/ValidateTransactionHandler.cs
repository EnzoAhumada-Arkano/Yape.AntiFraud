using Yape.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using Yape.Application.Antifraud.Commands;

namespace Yape.Application.Antifraud.Handlers
{
    public class ValidateTransactionHandler : IRequestHandler<ValidateTransactionCommand, bool>
    {
        private readonly ILogger<ValidateTransactionHandler> _logger;
        private readonly ITransactionRepository _transactionRepository;

        public ValidateTransactionHandler(ILogger<ValidateTransactionHandler> logger, ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
        }
        public async Task<bool> Handle(ValidateTransactionCommand request, CancellationToken cancellationToken)
        {
            bool isValid = true;
            _logger.LogInformation("ValidateTransactionHandler:Handle - Logic");
            var transaction = await _transactionRepository.GetTransactionsByExternalIdAsync(request.TransactionExternalId);

            //Validate value of the transaction
            if (transaction != null && transaction.Value > 2000)
            {
                _logger.LogWarning("Transaction value exceeds limit");
                isValid = false;
            }

            //Validate total transactions by user current day
            var transactions = await _transactionRepository.GetTransactionsSenderByAccountIdAsync(request.SourceAccountId);
            var transactionsToday = transactions.Where(x => x.CreatedAt.Date == DateTime.UtcNow.Date).ToList();
            //Sum the value of the transactions
            var totalValue = transactionsToday.Sum(x => x.Value);
            if (totalValue > 20000)
            {
                _logger.LogWarning("Total transaction value exceeds limit");
                isValid = false;
            }

            return isValid;
        }
    }
}
