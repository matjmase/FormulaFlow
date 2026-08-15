using FormulaFlow.Data.Enum;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;

namespace FormulaFlow.Server.Intermediate.Model.Parameter
{
    public class BooleanInputIntermediateParameter : IntermediateParameter
    {
        public override NetworkParameterType Type => NetworkParameterType.BooleanInput;

        public override string Description => "Boolean Input";

        public override string? ToolTip => null;

        public BooleanInputIntermediateParameter(int order) : base(order)
        { }
    }
}
