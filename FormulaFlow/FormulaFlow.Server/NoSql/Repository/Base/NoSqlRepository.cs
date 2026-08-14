using FormulaFlow.Data.NoSql;
using FormulaFlow.Data.NoSql.Models.Base;
using System.Linq.Expressions;

namespace FormulaFlow.Server.NoSql.Repository.Base
{
    public abstract class NoSqlRepository<TEntity> : INoSqlRepository<TEntity>
        where TEntity : NoSqlBaseIdEntityModel
    {
        protected virtual Expression<Func<TEntity, object>> _keySelector { get; } = e => e.Id;
        protected abstract Expression<Func<TEntity, object>> _orderByKeySelector { get; }


        private readonly NoSqlFormulaFlowContext _dbContext;

        public NoSqlRepository(
            NoSqlFormulaFlowContext dbContext
            )
        {
            _dbContext = dbContext;
        }

        public Task<TEntity> AddAsync(TEntity entity, string userId)
        {
            entity.UpdatedByUserId = userId;
            entity.CreatedByUserId = userId;

            _dbContext.GetCollection<TEntity>().Insert(entity);
            return Task.FromResult(entity);
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            var total = _dbContext.GetCollection<TEntity>().Query().Where(predicate).Count();

            return Task.FromResult(total);
        }

        public Task Delete(Guid id)
        {
            var entity = GetByIdAsync(id);

            if (entity == null)
            {
                throw new NullReferenceException();
            }

            var collection = _dbContext.GetCollection<TEntity>();
            collection.Delete(entity.Id);

            return Task.CompletedTask;
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            predicate = predicate ?? (e => true);

            var entities = _dbContext.GetCollection<TEntity>().Query().Where(predicate);

            entities = entities.OrderByDescending(_orderByKeySelector);

            var outputEntities = entities
                .ToEnumerable();

            return Task.FromResult(outputEntities);
        }

        public Task<TEntity?> GetByIdAsync(Guid id)
        {
            var entity = _dbContext.GetCollection<TEntity>().Query().Where(item => ((Guid)_keySelector.Compile().Invoke(item)) == id).FirstOrDefault();

            var output = entity == default(TEntity) ? null : entity;

            return Task.FromResult(output);
        }

        public Task<IEnumerable<TEntity>> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<TEntity, bool>>? predicate = null)
        {
            predicate = predicate ?? (e => true);

            var entities = _dbContext.GetCollection<TEntity>().Query().Where(predicate);

            entities = entities.OrderByDescending(_orderByKeySelector);

            var dtos = entities
                .Offset(pageIndex * pageSize)
                .Limit(pageSize)
                .ToEnumerable();

            return Task.FromResult<IEnumerable<TEntity>>(dtos);
        }

        public Task<TEntity> Update(Guid id, TEntity entity, string userId)
        {
            entity.Id = id;

            entity.UpdatedByUserId = userId;

            var collection = _dbContext.GetCollection<TEntity>();

            collection.Update(entity);

            return Task.FromResult(entity);
        }
    }
}
