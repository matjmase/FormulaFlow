using FormulaFlow.Data;
using FormulaFlow.Data.Models;
using FormulaFlow.Data.NoSql;
using FormulaFlow.Data.NoSql.Models;
using FormulaFlow.Server.Repository.Base;

namespace FormulaFlow.Server.Repository
{
    public class StockSymbolRepository : RepositoryBase<StockSymbol>, IRepository<StockSymbol>
    {

        private readonly NoSqlFormulaFlowContext _noSqlContext;

        public StockSymbolRepository(FormulaFlowContext context, NoSqlFormulaFlowContext noSqlContext) : base(context)
        {
            _noSqlContext = noSqlContext;
        }

        public override void Delete(StockSymbol entity)
        {
            _noSqlContext.GetCollection<StockDataEntry>().DeleteMany(entry => entry.StockSymbolId == entity.Id);

            base.Delete(entity);
        }

        public override Task Delete(Guid entityId)
        {
            _noSqlContext.GetCollection<StockDataEntry>().DeleteMany(entry => entry.StockSymbolId == entityId);

            return base.Delete(entityId);
        }
    }
}
