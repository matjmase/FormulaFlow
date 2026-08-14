using FormulaFlow.Server.Intermediate.Dto;
using FormulaFlow.Server.Intermediate.Model.Canvas.Base;
using FormulaFlow.Server.Mapper.Base;

namespace FormulaFlow.Server.Intermediate.Mapper.Frontend
{
    public class IntermediateDtoCanvasMapper : IMapper<IntermediateCanvas, StockCanvasDto>, IMapper<StockCanvasDto, IntermediateCanvas>
    {
        public StockCanvasDto Map(IntermediateCanvas from)
        {
            return new StockCanvasDto
            {
                Id = from.Id,
                Name = from.Name,
                Scale = from.Scale,
                Height = from.Height,
                Width = from.Width,
            };
        }

        public IntermediateCanvas Map(StockCanvasDto from)
        {
            return new IntermediateCanvas
            {
                Id = from.Id,
                Name = from.Name,
                Scale = from.Scale,
                Height = from.Height,
                Width = from.Width,
            };
        }
    }
}
