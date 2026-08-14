using FormulaFlow.Data.NoSql.Models;
using FormulaFlow.Server.Dto;
using FormulaFlow.Server.Mapper.Base;

namespace FormulaFlow.Server.Mapper
{
    public class StockDataEntryMapper : IMapper<StockDataEntryDto, StockDataEntry>, IMapper<StockDataEntry, StockDataEntryDto>
    {
        public StockDataEntry Map(StockDataEntryDto from)
        {
            return new StockDataEntry
            {
                Id = from.Id ?? Guid.Empty,
                StockSymbolId = from.StockSymbolId,
                Date = from.Date,
                Amount = from.Amount,
                CreatedByUserId = from.CreatedByUserId,
                UpdatedByUserId = from.UpdatedByUserId,
            };
        }

        public StockDataEntryDto Map(StockDataEntry from)
        {
            return new StockDataEntryDto
            {
                Id = from.Id,
                StockSymbolId = from.StockSymbolId,
                Date = from.Date,
                Amount = from.Amount,
                CreatedByUserId = from.CreatedByUserId,
                UpdatedByUserId = from.UpdatedByUserId,
            };
        }
    }
}
