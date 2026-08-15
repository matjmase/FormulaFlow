using FormulaFlow.Data.Enum;
using FormulaFlow.Server.Intermediate.Formula;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;

namespace FormulaFlow.Server.Intermediate.Model.Parameter
{
    public class TransitionFormulaIntermediateParameter : IntermediateParameter
    {
        public readonly ContainerRegexFormula[] ContainerOperators = RegexFormulaParsing.Containers;
        public readonly VariableRegexFormula[] VariableOperators = RegexFormulaParsing.Variables;
        public readonly UnaryRegexFormula[] NumericUnary = RegexFormulaParsing.NumericUnary;
        public readonly UnaryRegexFormula[] BooleanUnary = RegexFormulaParsing.BooleanUnary;
        public readonly BinaryRegexFormula[] TransBinary = RegexFormulaParsing.TransitionBinary;

        public RegexFormula[][] Operators;

        public override NetworkParameterType Type => NetworkParameterType.Transitional;

        public override string Description => "Transition Formula";

        private string? _toolTip = null;
        public override string? ToolTip => _toolTip;

        public TransitionFormulaIntermediateParameter(int order) : base(order)
        {
            Operators = [
                    ContainerOperators,
                    VariableOperators,
                    NumericUnary,
                    BooleanUnary,
                    TransBinary,
                ];

            _toolTip = string.Join("\n", Operators.SelectMany(arr => arr.Select(op => $"{op.Symbol} - {op.Description}")));
        }
    }
}
