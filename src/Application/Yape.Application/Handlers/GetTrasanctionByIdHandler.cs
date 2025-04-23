using Application.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Yape.Infrastructure.Postgresql.Database;

namespace Application.Handlers
{
    public class GetTrasanctionByIdHandler : IRequestHandler<GetTransactionByIdQuery, Transaction>
    {
        private readonly AppDbContext _dbContext;

        public GetTrasanctionByIdHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Transaction> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _dbContext.Transactions.Where(x => x.TransactionExternalId == request.TransactionExternalId).FirstOrDefaultAsync();
            if (transaction == null)
            {
                return null;
            }

            // Map the transaction infrastructure model to the domain model
            var transactionDomain = new Transaction
            {
                TransactionExternalId = transaction.TransactionExternalId,
                CreatedAt = transaction.CreatedAt,
                SourceAccountId = transaction.SourceAccountId,
                TargetAccountId = transaction.TargetAccountId,
                TransferTypeId = transaction.TransferTypeId,
                Value = transaction.Value
            };

            return transactionDomain;
        }
    }
}
