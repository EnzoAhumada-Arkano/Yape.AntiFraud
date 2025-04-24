using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.Infrastructure.Postgresql.Database;

namespace Yape.Infrastructure.Postgresql.Repository
{
    public class BaseRepository<TEntity> : IDisposable, IBaseRepository<TEntity> where TEntity : class
    {
        private readonly ILogger<BaseRepository<TEntity>> _logger;
        private readonly AppDbContext _dbContext;

        public BaseRepository(ILogger<BaseRepository<TEntity>> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task AddAsync(TEntity entity)
        {
            try
            {
                await _dbContext.Set<TEntity>().AddAsync(entity);
                await SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "AddAsync - Error occurred while adding entity.");
            }
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Set<TEntity>().CountAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbContext.Set<TEntity>().Remove(entity);
                await SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning($"DeleteAsync - Entity with ID {id} not found.");
            }
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            bool isUpdated = false;
            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                await SaveChangesAsync();
                isUpdated = true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                isUpdated = false;
                _logger.LogError(ex, "UpdateAsync - Error occurred while updating entity.");
            }

            return isUpdated;
        }
    }
}
