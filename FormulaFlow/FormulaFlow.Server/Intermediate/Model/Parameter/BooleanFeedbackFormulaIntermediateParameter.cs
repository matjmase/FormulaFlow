using FormulaFlow.Data.Enum;
using FormulaFlow.Server.Intermediate.Formula;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;

namespace FormulaFlow.Server.Intermediate.Model.Parameter
{
    public class BooleanFeedbackFormulaIntermediateParameter : IntermediateParameter
    {
        public readonly ContainerRegexFormula[] ContainerOperators = RegexFormulaParsing.Containers;
        public readonly VariableRegexFormula[] VariableOperators = RegexFormulaParsing.Variables;
        public readonly VariableRegexFormula[] VariableFeedbackOperators = RegexFormulaParsing.VariablesFeedback;
        public readonly UnaryRegexFormula[] BooleanUnary = RegexFormulaParsing.BooleanUnary;
        public readonly BinaryRegexFormula[] BooleanBinary = RegexFormulaParsing.BooleanBinary;

        public RegexFormula[][] Operators;

        public override NetworkParameterType Type => NetworkParameterType.BooleanFeedback;

        public override string Description => "Boolean Feedback Formula";

        private string? _toolTip = null;
        public override string? ToolTip => _toolTip;

        public BooleanFeedbackFormulaIntermediateParameter(int order) : base(order)
        {
            Operators = [
                    ContainerOperators,
                    VariableOperators,
                    VariableFeedbackOperators,
                    BooleanUnary,
                    BooleanBinary,
                ];

            _toolTip = string.Join("\n", Operators.SelectMany(arr => arr.Select(op => $"{op.Symbol} - {op.Description}")));
        }
    }
}
