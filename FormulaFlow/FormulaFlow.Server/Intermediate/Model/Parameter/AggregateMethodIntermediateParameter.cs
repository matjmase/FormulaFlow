using FormulaFlow.Data.Enum;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;

namespace FormulaFlow.Server.Intermediate.Model.Parameter
{
    public class AggregateMethodIntermediateParameter : IntermediateParameter
    {
        public override NetworkParameterType Type => NetworkParameterType.AggregateMethod;

        public override string Description => "Aggregate Method";

        public override string? ToolTip => null;

        public AggregateMethodIntermediateParameter(int order) : base(order)
        {
        }
    }

    public enum AggregateMethodIntermediateParameterType
    {
        Summation,
        Average,
        Multiplicative
    }
}
