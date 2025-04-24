using Domain.Entities;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.Infrastructure.Postgresql.Database;

namespace Yape.Infrastructure.Postgresql.Repository
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        private readonly ILogger<TransactionRepository> _logger;
        private readonly AppDbContext _dbContext;

        public TransactionRepository(ILogger<TransactionRepository> logger, AppDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public async Task<Transaction> GetTransactionsByExternalIdAsync(Guid externalId)
        {
            return await _dbContext.Transactions.Where(x => x.TransactionExternalId == externalId).FirstOrDefaultAsync(); 
        }

        public Task<IEnumerable<Transaction>> GetTransactionsByTransferTypeIdAsync(int transferTypeId)
        {
            throw new NotImplementedException();
        }
    }

}
