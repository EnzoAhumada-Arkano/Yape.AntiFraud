using Application.Commands;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using Yape.Infrastructure.Postgresql.Database;


namespace Application.Handlers
{
    public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, Guid>
    {
        private readonly ILogger<CreateTransactionHandler> _logger;
        private readonly ITransactionRepository _transactionRepository;

        public CreateTransactionHandler(ILogger<CreateTransactionHandler> logger, ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
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
                TargetAccountId = request.DestinationAccountId,
                TransferTypeId = request.TransferTypeId,
                Value = request.Value,
            };
            // Save to database
            await _transactionRepository.AddAsync(transaction);
            return transaction.TransactionExternalId;
        }
    }
}
