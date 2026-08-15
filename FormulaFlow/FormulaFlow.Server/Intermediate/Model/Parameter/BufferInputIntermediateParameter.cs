using FormulaFlow.Data.Enum;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;

namespace FormulaFlow.Server.Intermediate.Model.Parameter
{
    public class BufferInputIntermediateParameter : IntermediateParameter
    {
        public override NetworkParameterType Type => NetworkParameterType.BufferInput;

        public override string Description => "Buffer Amount";

        public override string? ToolTip => null;

        public BufferInputIntermediateParameter(int order) : base(order)
        { }
    }
}
