using FormulaFlow.Data.Models.Base;
using System.Linq.Expressions;

namespace FormulaFlow.Server.Service.Base
{
    public interface IServiceBase<TEntity, TDto> where TEntity : BaseIdEntityModel
    {
        Task<TDto?> GetByIdAsync(Guid id);
        Task<PagedData<TDto>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);
        Task<TDto> AddAsync(TDto dto, string UserId);
        Task<TDto> Update(Guid id, TDto dto, string UserId);
        Task Delete(Guid id);
    }
}
