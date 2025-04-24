using Yape.Domain.Entities;
using Yape.Domain.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yape.Infrastructure.Postgresql.Database;
using Yape.Application.Transaction.Queries;

namespace Yape.Application.Transaction.Handlers
{
    public class GetTrasanctionByExternalIdHandler : IRequestHandler<GetTransactionByExternalIdQuery, Yape.Domain.Entities.Transaction>
    {
        private readonly ILogger<GetTrasanctionByExternalIdHandler> _logger;
        private readonly ITransactionRepository _transactionRepository;

        public GetTrasanctionByExternalIdHandler(ILogger<GetTrasanctionByExternalIdHandler> logger, ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
        }

        public async Task<Yape.Domain.Entities.Transaction> Handle(GetTransactionByExternalIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetTrasanctionByExternalIdHandler:Handle - Logic");
            var transaction = await _transactionRepository.GetTransactionsByExternalIdAsync(request.TransactionExternalId);

            if (transaction != null)
            {
                // Map the transaction infrastructure model to the domain model
                var transactionDomain = new Yape.Domain.Entities.Transaction
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
            else
            {
                return null;
            }

            
        }
    }
}
