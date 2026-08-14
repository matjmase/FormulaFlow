using FormulaFlow.Data;
using FormulaFlow.Data.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace FormulaFlow.Server.Repository.Base
{
    public class OwnerRepository<TEntity> : RepositoryBase<TEntity>, IRepository<TEntity>
        where TEntity : OwnerEntityModel
    {
        public OwnerRepository(FormulaFlowContext context) : base(context)
        {
        }

        public override Task<TEntity> AddAsync(TEntity entity, string userId)
        {
            entity.OwnerUserId = userId;

            return base.AddAsync(entity, userId);
        }

        public override async Task<TEntity> Update(TEntity updateEntity, string userId)
        {
            updateEntity.UpdatedByUserId = userId;

            _dbSet.Attach(updateEntity);
            var tracked = _dbSet.Update(updateEntity);

            tracked.Property(entity => entity.CreatedByUserId).IsModified = false;
            tracked.Property(entity => entity.OwnerUserId).IsModified = false;

            return tracked.Entity;
        }
    }
}
