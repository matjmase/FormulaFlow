using FormulaFlow.Data.NoSql.Models.Base;
using FormulaFlow.Server.Mapper.Base;
using FormulaFlow.Server.NoSql.Repository.Base;
using FormulaFlow.Server.Service.Base;
using System.Linq.Expressions;

namespace FormulaFlow.Server.NoSql.Service.Base
{
    public class NoSqlService<TEntity, TDto> : INoSqlService<TEntity, TDto> where TEntity : NoSqlBaseIdEntityModel
    {
        private readonly INoSqlRepository<TEntity> _repository;
        private readonly IMapper<TDto, TEntity> _mapperIn;
        private readonly IMapper<TEntity, TDto> _mapperOut;

        public NoSqlService(
            INoSqlRepository<TEntity> repository,
            IMapper<TDto, TEntity> mapperIn,
            IMapper<TEntity, TDto> mapperOut
            )
        {
            _repository = repository;
            _mapperIn = mapperIn;
            _mapperOut = mapperOut;
        }
        public async Task<TDto> AddAsync(TDto dto, string UserId)
        {
            var entity = await _repository.AddAsync(_mapperIn.Map(dto), UserId);

            return _mapperOut.Map(entity);
        }

        public async Task Delete(Guid id)
        {
            await _repository.Delete(id);
        }

        public async Task<TDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? default(TDto) : _mapperOut.Map(entity);
        }

        public async Task<IEnumerable<TDto>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            predicate = predicate ?? (e => true);

            var entities = await _repository.GetAllAsync(predicate);

            return entities.Select(entity => _mapperOut.Map(entity));
        }

        public async Task<PagedData<TDto>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>>? predicate = null)
        {
            predicate = predicate ?? (e => true);

            var entities = await _repository.GetPagedAsync(pageIndex, pageSize, predicate);

            var dtos = entities.Select(entity => _mapperOut.Map(entity));

            var count = await _repository.CountAsync(predicate);

            return new PagedData<TDto>
            {
                Record = dtos,
                Page = pageIndex,
                PageSize = pageSize,
                RecordCount = count
            };
        }

        public async Task<TDto> Update(Guid id, TDto dto, string UserId)
        {
            var entity = await _repository.Update(id, _mapperIn.Map(dto), UserId);

            return _mapperOut.Map(entity);
        }
    }
}
