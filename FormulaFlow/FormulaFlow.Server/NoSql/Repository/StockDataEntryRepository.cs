using FormulaFlow.Data.NoSql;
using FormulaFlow.Data.NoSql.Models;
using FormulaFlow.Server.NoSql.Repository.Base;
using System.Linq.Expressions;

namespace FormulaFlow.Server.NoSql.Repository
{
    public class StockDataEntryRepository : NoSqlRepository<StockDataEntry>
    {
        public StockDataEntryRepository(NoSqlFormulaFlowContext dbContext) : base(dbContext)
        {
        }

        protected override Expression<Func<StockDataEntry, object>> _orderByKeySelector => e => e.Date;
    }
}
