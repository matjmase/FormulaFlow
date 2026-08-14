using FormulaFlow.Data.Models;
using FormulaFlow.Server.Dto;
using FormulaFlow.Server.Mapper.Base;

namespace FormulaFlow.Server.Mapper
{
    public class StockSymbolMapper : IMapper<StockSymbolDto, StockSymbol>, IMapper<StockSymbol, StockSymbolDto>
    {
        public StockSymbol Map(StockSymbolDto from)
        {
            return new StockSymbol
            {
                Id = from.Id ?? Guid.Empty,
                Symbol = from.Symbol,
                CreatedByUserId = from.CreatedByUserId,
                UpdatedByUserId = from.UpdatedByUserId,
            };
        }

        public StockSymbolDto Map(StockSymbol from)
        {
            return new StockSymbolDto
            {
                Id = from.Id,
                Symbol = from.Symbol,
                CreatedByUserId = from.CreatedByUserId,
                UpdatedByUserId = from.UpdatedByUserId,
            };
        }
    }
}
