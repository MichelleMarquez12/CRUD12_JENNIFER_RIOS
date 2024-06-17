using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportManager.DataAccess.Repositories
{
    public class Repository<TId, TEntity> : IRepository<TId, TEntity> where TEntity : class, new()
    {
        private readonly TransportManagerContext _context;

        public Repository(TransportManagerContext context)
        {
            _context = context;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public virtual async Task DeleteAsync(TId id)
        {
            var entity = await _context.FindAsync<TEntity>(id);
            if (entity == null)
            {
                throw new Exception("Entity not found");
            }
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            try
            {
                return _context.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public virtual async Task<TEntity> GetAsync(TId id)
        {
            var entity = await _context.FindAsync<TEntity>(id);
            if (entity == null)
            {
                throw new Exception("Entity not found");
            }
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(UpdateAsync)} entity must not be null");
            }

            try
            {
                _context.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }
    }
}
