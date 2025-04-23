using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        /// Define the methods that all repositories should implement
        Task AddAsync<TEntity>(TEntity entity) where TEntity : class;
        Task<TEntity> GetByIdAsync<TEntity>(int id) where TEntity : class;
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>() where TEntity : class;
        Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class;
        Task DeleteAsync<TEntity>(int id) where TEntity : class;
        Task SaveChangesAsync();
    }
}
