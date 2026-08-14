using FormulaFlow.Data.NoSql.Models.Base;
using FormulaFlow.Server.Service.Base;
using System.Linq.Expressions;

namespace FormulaFlow.Server.NoSql.Service.Base
{
    public interface INoSqlService<TEntity, TDto> where TEntity : NoSqlBaseIdEntityModel
    {
        Task<TDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<TDto>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null);
        Task<PagedData<TDto>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>>? predicate = null);
        Task<TDto> AddAsync(TDto dto, string UserId);
        Task<TDto> Update(Guid id, TDto dto, string UserId);
        Task Delete(Guid id);
    }
}
