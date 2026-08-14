using FormulaFlow.Data;
using FormulaFlow.Data.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FormulaFlow.Server.Repository.Base
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : BaseIdEntityModel
    {
        protected readonly FormulaFlowContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(FormulaFlowContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            if (pageIndex < 0) pageIndex = 0;
            if (pageSize <= 0) pageSize = 10;

            var query = _dbSet.AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            else
            {
                query = query.OrderBy(e => e.Id);
            }

            var total = await query.CountAsync();

            var items = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToArrayAsync();

            return items;
        }
        public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? predicate)
        {
            var query = _dbSet.AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.CountAsync();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, string userId)
        {
            entity.CreatedByUserId = userId;
            entity.UpdatedByUserId = userId;

            await _dbSet.AddAsync(entity);

            return entity;
        }

        public virtual async Task<TEntity> Update(TEntity updateEntity, string userId)
        {
            updateEntity.UpdatedByUserId = userId;

            _dbSet.Attach(updateEntity);
            var tracked = _dbSet.Update(updateEntity);

            tracked.Property(entity => entity.CreatedByUserId).IsModified = false;

            return tracked.Entity;
        }

        public virtual async void Delete(TEntity entityId)
        {
            _dbSet.Remove(entityId);
        }

        public virtual async Task Delete(Guid entityId)
        {
            var entity = await GetByIdAsync(entityId);

            if (entity == null)
            {
                throw new ArgumentException($"Entity with id {entityId} not found for deletion.");
            }

            _dbSet.Remove(entity);
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            var query = _dbSet.AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query
                .ToArrayAsync();
        }
    }
}
