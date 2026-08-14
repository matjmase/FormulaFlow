using FormulaFlow.Server.Intermediate.Dto;

namespace FormulaFlow.Server.Service
{
    public interface ICardCatalogService
    {
        public Task<IEnumerable<StockCardDto>> Get();
    }
}
