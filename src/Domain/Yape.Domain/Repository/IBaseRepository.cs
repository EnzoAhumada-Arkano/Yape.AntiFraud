using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yape.Domain.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Add a new entity to the database.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddAsync(TEntity entity);
        /// <summary>
        /// Get an entity by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(int id);
        /// <summary>
        /// GET all entities from the database.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync();
        /// <summary>
        /// Update an existing entity in the database.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity);
        /// <summary>
        /// Delete an entity from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(int id);
        /// <summary>
        /// Save changes to the database.
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();
        /// <summary>
        /// Count the number of entities in the database.
        /// </summary>
        /// <returns></returns>
        Task<int> CountAsync();
    }
}
