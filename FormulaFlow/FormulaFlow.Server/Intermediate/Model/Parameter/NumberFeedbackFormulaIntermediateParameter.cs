using FormulaFlow.Data.Enum;
using FormulaFlow.Server.Intermediate.Formula;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;

namespace FormulaFlow.Server.Intermediate.Model.Parameter
{
    public class NumberFeedbackFormulaIntermediateParameter : IntermediateParameter
    {
        public readonly ContainerRegexFormula[] ContainerOperators = RegexFormulaParsing.Containers;
        public readonly VariableRegexFormula[] VariableOperators = RegexFormulaParsing.Variables;
        public readonly VariableRegexFormula[] VariableFeedbackOperators = RegexFormulaParsing.VariablesFeedback;
        public readonly UnaryRegexFormula[] NumericUnary = RegexFormulaParsing.NumericUnary;
        public readonly BinaryRegexFormula[] NumericBinary = RegexFormulaParsing.NumericBinary;

        public RegexFormula[][] Operators;

        public override NetworkParameterType Type => NetworkParameterType.NumberFeedback;

        public override string Description => "Number Feedback Formula";

        private string? _toolTip = null;
        public override string? ToolTip => _toolTip;

        public NumberFeedbackFormulaIntermediateParameter(int order) : base(order)
        {
            Operators = [
                    ContainerOperators,
                    VariableOperators,
                    VariableFeedbackOperators,
                    NumericUnary,
                    NumericBinary,
                ];

            _toolTip = string.Join("\n", Operators.SelectMany(arr => arr.Select(op => $"{op.Symbol} - {op.Description}")));
        }
    }
}
