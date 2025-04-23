using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface ITransactionRepository : IRepositoryBase
    {
        // Define any additional methods specific to the Transaction repository here
        Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(Guid accountId);
        Task<IEnumerable<Transaction>> GetTransactionsByTransferTypeIdAsync(int transferTypeId);
        Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
