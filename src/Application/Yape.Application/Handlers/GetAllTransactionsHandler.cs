using Application.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Yape.Infrastructure.Postgresql.Database;


namespace Application.Handlers
{
    public class GetAllTransactionsHandler : IRequestHandler<GetAllTransactionsQuery, List<Transaction>>
    {
        private readonly AppDbContext _dbContext;

        public GetAllTransactionsHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Transaction>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            List<Transaction> transactionsRetrieve = new List<Transaction>();
            // Fetch transactions from the database
            var transactions = await _dbContext.Transactions.ToListAsync(cancellationToken);
            foreach (var transaction in transactions)
            {
                // Map the transaction to TransactionRetrieve
                var transactionRetrieve = new Transaction
                {
                    TransactionExternalId = transaction.TransactionExternalId,
                    CreatedAt = transaction.CreatedAt,
                    SourceAccountId = transaction.SourceAccountId,
                    TargetAccountId = transaction.TargetAccountId,
                    TransferTypeId = transaction.TransferTypeId,
                    Value = transaction.Value
                };

                transactionsRetrieve.Add(transactionRetrieve);
            }

            return transactionsRetrieve;

        }
    }
}
