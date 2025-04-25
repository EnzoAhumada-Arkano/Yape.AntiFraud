using Yape.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Yape.Infrastructure.Postgresql.Database;
using Yape.Application.Transaction.Queries;
using Domain.Models;
using Microsoft.Extensions.Logging;


namespace Yape.Application.Transaction.Handlers
{
    public class GetAllTransactionsHandler : IRequestHandler<GetAllTransactionsQuery, List<TransactionRetrieve>>
    {
        private readonly ILogger<GetAllTransactionsHandler> _logger;
        private readonly AppDbContext _dbContext;

        public GetAllTransactionsHandler(ILogger<GetAllTransactionsHandler> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<List<TransactionRetrieve>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetAllTransactionsHandler:Handle - Logic");
            List<TransactionRetrieve> transactionsRetrieve = new List<TransactionRetrieve>();
            // Fetch transactions from the database
            var transactions = await _dbContext.Transactions.ToListAsync(cancellationToken);
            foreach (var transaction in transactions)
            {
                // Map the transaction to TransactionRetrieve
                var transactionRetrieve = new TransactionRetrieve()
                {
                    TransactionExternalId = transaction.TransactionExternalId,
                    CreatedAt = transaction.CreatedAt,
                };

                transactionsRetrieve.Add(transactionRetrieve);
            }

            return transactionsRetrieve;

        }
    }
}
