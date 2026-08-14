using FormulaFlow.Data.NoSql.Models.Base;
using System.Linq.Expressions;

namespace FormulaFlow.Server.NoSql.Repository.Base
{
    public interface INoSqlRepository<TEntity> where TEntity : NoSqlBaseIdEntityModel
    {
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null);
        Task<IEnumerable<TEntity>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>>? predicate = null);
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);
        Task<TEntity> AddAsync(TEntity entity, string userId);
        Task<TEntity> Update(Guid id, TEntity entity, string userId);
        Task Delete(Guid id);
    }
}
