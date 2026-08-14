using FormulaFlow.Data.NoSql.Models;
using FormulaFlow.Server.Dto;
using FormulaFlow.Server.Mapper.Base;
using FormulaFlow.Server.NoSql.Repository.Base;
using FormulaFlow.Server.NoSql.Service.Base;

namespace FormulaFlow.Server.NoSql.Service
{
    public class StockDataEntryService : NoSqlService<StockDataEntry, StockDataEntryDto>, INoSqlService<StockDataEntry, StockDataEntryDto>
    {
        public StockDataEntryService(INoSqlRepository<StockDataEntry> repository, IMapper<StockDataEntryDto, StockDataEntry> mapperIn, IMapper<StockDataEntry, StockDataEntryDto> mapperOut) : base(repository, mapperIn, mapperOut)
        {
        }
    }
}
