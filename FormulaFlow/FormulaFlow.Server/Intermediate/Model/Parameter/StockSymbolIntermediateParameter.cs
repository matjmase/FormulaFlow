using FormulaFlow.Data.Enum;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;

namespace FormulaFlow.Server.Intermediate.Model.Parameter
{
    public class StockSymbolIntermediateParameter : IntermediateParameter
    {
        public override NetworkParameterType Type => NetworkParameterType.StockSource;

        public override string Description => "Stock data source";

        public override string? ToolTip => null;

        public StockSymbolIntermediateParameter(int order) : base(order)
        { }
    }
}
