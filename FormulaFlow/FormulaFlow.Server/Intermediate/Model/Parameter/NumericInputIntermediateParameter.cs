using FormulaFlow.Data.Enum;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;

namespace FormulaFlow.Server.Intermediate.Model.Parameter
{
    public class NumericInputIntermediateParameter : IntermediateParameter
    {
        public override NetworkParameterType Type => NetworkParameterType.NumericInput;

        public override string Description => "Number Input";

        public override string? ToolTip => null;

        public NumericInputIntermediateParameter(int order) : base(order)
        { }
    }
}
