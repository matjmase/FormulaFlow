using FormulaFlow.Data.Models.Base;
using FormulaFlow.Server.Dto.Base;
using FormulaFlow.Server.Mapper.Base;
using FormulaFlow.Server.Repository.Base;
using System.Linq.Expressions;

namespace FormulaFlow.Server.Service.Base
{
    public class ServiceBase<TEntity, TDto> : IServiceBase<TEntity, TDto> where TEntity : BaseIdEntityModel
        where TDto : BaseIdDtoModel
    {

        private readonly IRepository<TEntity> _repo;
        private readonly IMapper<TDto, TEntity> _mapperIn;
        private readonly IMapper<TEntity, TDto> _mapperOut;

        public ServiceBase(
            IRepository<TEntity> repo,
            IMapper<TDto, TEntity> mapperIn,
            IMapper<TEntity, TDto> mapperOut
            )
        {
            _repo = repo;
            _mapperIn = mapperIn;
            _mapperOut = mapperOut;
        }

        public async Task<TDto> AddAsync(TDto dto, string userId)
        {
            var entity = _mapperIn.Map(dto);

            await _repo.AddAsync(entity, userId);

            await _repo.SaveChangesAsync();

            return _mapperOut.Map(entity);
        }

        public async Task Delete(Guid id)
        {
            await _repo.Delete(id);
            await _repo.SaveChangesAsync();
        }

        public async Task<TDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);

            if (entity == null)
            {
                return default(TDto);
            }

            return _mapperOut.Map(entity);
        }

        public async Task<PagedData<TDto>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            var entities = await _repo.GetPagedAsync(pageIndex, pageSize, predicate, orderBy);

            var dtos = entities.Select(e => _mapperOut.Map(e)).ToList();

            var totalCount = await _repo.GetCountAsync(predicate);

            return new PagedData<TDto>
            {
                Record = dtos,
                Page = pageIndex,
                PageSize = pageSize,
                RecordCount = totalCount
            };
        }

        public async Task<TDto> Update(Guid id, TDto dto, string userId)
        {
            var entity = _mapperIn.Map(dto);

            entity.Id = id;

            await _repo.Update(entity, userId);

            await _repo.SaveChangesAsync();

            return _mapperOut.Map(entity);
        }
    }
}
