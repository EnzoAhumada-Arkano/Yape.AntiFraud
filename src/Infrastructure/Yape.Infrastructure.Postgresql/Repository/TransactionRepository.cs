using Domain.Entities;
using Domain.Repository;
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
        private readonly AppDbContext _dbContext;

        public TransactionRepository(AppDbContext dbContext) : base(dbContext)
        {
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

        public Task<IEnumerable<Transaction>> GetTransactionsByTransferTypeIdAsync(int transferTypeId)
        {
            throw new NotImplementedException();
        }
    }

}
