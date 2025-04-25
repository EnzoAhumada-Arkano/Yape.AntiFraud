using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.Application.Transaction.Commands;
using Yape.Domain.Repository;

namespace Yape.Application.Transaction.Handlers
{
    public class PatchTransanctionStatusHandler : IRequestHandler<PatchTransactionStatusCommand, Guid>
    {
        private readonly ILogger<PatchTransanctionStatusHandler> _logger;
        private readonly ITransactionRepository _transactionRepository;

        public PatchTransanctionStatusHandler(ILogger<PatchTransanctionStatusHandler> logger, ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
        }
        public async Task<Guid> Handle(PatchTransactionStatusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("PatchTransanctionStatusHandler:Handle - Logic");
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var transaction = await _transactionRepository.GetTransactionsByExternalIdAsync(request.TransactionExternalId);
            if (transaction != null)
            {
                transaction.Status = request.Status;
                await _transactionRepository.UpdateAsync(transaction);
            }

            return transaction.TransactionExternalId;
        }
    }
}
