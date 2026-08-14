using FormulaFlow.Server.Dto.Base;

namespace FormulaFlow.Server.Dto
{
    public class StockCanvasSimpleDto : BaseIdDtoModel
    {
        public string Name { get; set; }
        public double Scale { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}
