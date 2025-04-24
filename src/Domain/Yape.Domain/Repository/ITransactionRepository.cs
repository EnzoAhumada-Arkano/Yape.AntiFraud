using Yape.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yape.Domain.Repository
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {
        // Define any additional methods specific to the Transaction repository here
        Task<IEnumerable<Transaction>> GetTransactionsSenderByAccountIdAsync(Guid accountId);
        Task<IEnumerable<Transaction>> GetTransactionsReceiverByAccountIdAsync(Guid accountId);
        Task<IEnumerable<Transaction>> GetTransactionsByTransferTypeIdAsync(int transferTypeId);
        Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Transaction> GetTransactionsByExternalIdAsync(Guid externalId);
    }
}
