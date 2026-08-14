using FormulaFlow.Data.Models.Base;
using System.Linq.Expressions;

namespace FormulaFlow.Server.Repository.Base
{
    public interface IRepository<TEntity> where TEntity : BaseIdEntityModel
    {
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null);
        Task<IEnumerable<TEntity>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? predicate = null);
        Task<TEntity> AddAsync(TEntity entity, string userId);
        Task<TEntity> Update(TEntity updateEntity, string userId);
        void Delete(TEntity entityId);
        Task Delete(Guid entityId);
        Task<int> SaveChangesAsync();
    }
}
