using FormulaFlow.Data.Enum;
using FormulaFlow.Server.Intermediate.Model.Base;

namespace FormulaFlow.Server.Intermediate.Model.Parameter.Base
{
    public abstract class IntermediateParameter : BaseIntermediateModel
    {
        public Guid NetworkCardId { get; set; }
        public abstract NetworkParameterType Type { get; }
        public abstract string Description { get; }
        public abstract string? ToolTip { get; }
        public int Order;
        public string Value { get; set; } = string.Empty;

        public IntermediateParameter()
        {
        }

        public IntermediateParameter(int order)
        {
            Order = order;
        }
    }
}
