using Application.Commands;

using MediatR;
using Yape.Infrastructure.Postgresql.Database;


namespace Application.Handlers
{
    public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, Guid>
    {
        private readonly AppDbContext _dbContext;

        public CreateTransactionHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            // Validate the command
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            // Map command to entity
            var transaction = new Domain.Entities.Transaction
            {
                SourceAccountId = request.SourceAccountId,
                TargetAccountId = request.DestinationAccountId,
                TransferTypeId = request.TransferTypeId,
                Value = request.Value,
            };
            // Save to database
            _dbContext.Transactions.Add(transaction);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return transaction.TransactionExternalId;
        }
    }
}
