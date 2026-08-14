using FormulaFlow.Server.Intermediate.Model.Base;

namespace FormulaFlow.Server.Intermediate.Model.Canvas.Base
{
    public class IntermediateCanvas : BaseIntermediateModel
    {
        public string Name { get; set; }
        public double Scale { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}
