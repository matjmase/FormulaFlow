using FormulaFlow.Data.Enum;
using FormulaFlow.Server.Intermediate.Formula;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;

namespace FormulaFlow.Server.Intermediate.Model.Parameter
{
    public class BooleanFormulaIntermediateParameter : IntermediateParameter
    {
        public readonly ContainerRegexFormula[] ContainerOperators = RegexFormulaParsing.Containers;
        public readonly VariableRegexFormula[] VariableOperators = RegexFormulaParsing.Variables;
        public readonly UnaryRegexFormula[] BooleanUnary = RegexFormulaParsing.BooleanUnary;
        public readonly BinaryRegexFormula[] BooleanBinary = RegexFormulaParsing.BooleanBinary;

        public RegexFormula[][] Operators;

        public override NetworkParameterType Type => NetworkParameterType.Boolean;

        public override string Description => "Boolean Formula";

        private string? _toolTip = null;
        public override string? ToolTip => _toolTip;

        public BooleanFormulaIntermediateParameter(int order) : base(order)
        {
            Operators = [
                    ContainerOperators,
                    VariableOperators,
                    BooleanUnary,
                    BooleanBinary,
                ];

            _toolTip = string.Join("\n", Operators.SelectMany(arr => arr.Select(op => $"{op.Symbol} - {op.Description}")));
        }
    }
}
