using Yape.Domain.Entities;
using Yape.Domain.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yape.Infrastructure.Postgresql.Database;
using Yape.Application.Transaction.Queries;
using Domain.Models;

namespace Yape.Application.Transaction.Handlers
{
    public class GetTrasanctionByExternalIdHandler : IRequestHandler<GetTransactionByExternalIdQuery, TransactionRetrieve>
    {
        private readonly ILogger<GetTrasanctionByExternalIdHandler> _logger;
        private readonly ITransactionRepository _transactionRepository;

        public GetTrasanctionByExternalIdHandler(ILogger<GetTrasanctionByExternalIdHandler> logger, ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
        }

        public async Task<TransactionRetrieve> Handle(GetTransactionByExternalIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetTrasanctionByExternalIdHandler:Handle - Logic");
            var transaction = await _transactionRepository.GetTransactionsByExternalIdAsync(request.TransactionExternalId);

            if (transaction != null)
            {
                // Map the transaction infrastructure model to the domain model
                var transactionRetrieve = new TransactionRetrieve
                {
                    TransactionExternalId = transaction.TransactionExternalId,
                    CreatedAt = transaction.CreatedAt,
                };

                return transactionRetrieve;
            }
            else
            {
                return null;
            }
        }
    }
}
