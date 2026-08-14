using FormulaFlow.Server.Intermediate.Dto;

namespace FormulaFlow.Server.Service
{
    public interface IStockCanvasCompositeService
    {
        public Task<StockCanvasDto> Add(StockCanvasDto addDto, string userId);
        public Task<StockCanvasDto> Get(Guid id);
        public Task<StockCanvasDto> Update(StockCanvasDto updateDto, string userId);
    }
}
