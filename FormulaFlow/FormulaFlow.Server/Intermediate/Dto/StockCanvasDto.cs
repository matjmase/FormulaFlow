using FormulaFlow.Server.Dto.Base;

namespace FormulaFlow.Server.Intermediate.Dto
{
    public class StockCanvasDto : BaseIdDtoModel
    {
        public string Name { get; set; }
        public double Scale { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public IEnumerable<StockCardDto> Cards { get; set; }
    }
}
