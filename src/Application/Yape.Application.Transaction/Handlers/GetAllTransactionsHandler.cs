using Yape.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Yape.Infrastructure.Postgresql.Database;
using Yape.Application.Transaction.Queries;


namespace Yape.Application.Transaction.Handlers
{
    public class GetAllTransactionsHandler : IRequestHandler<GetAllTransactionsQuery, List<Yape.Domain.Entities.Transaction>>
    {
        private readonly AppDbContext _dbContext;

        public GetAllTransactionsHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Yape.Domain.Entities.Transaction>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            List<Yape.Domain.Entities.Transaction> transactionsRetrieve = new List<Yape.Domain.Entities.Transaction>();
            // Fetch transactions from the database
            var transactions = await _dbContext.Transactions.ToListAsync(cancellationToken);
            foreach (var transaction in transactions)
            {
                // Map the transaction to TransactionRetrieve
                var transactionRetrieve = new Yape.Domain.Entities.Transaction
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
