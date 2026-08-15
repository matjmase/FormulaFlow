using FormulaFlow.Data.Enum;
using FormulaFlow.Server.Intermediate.Formula;
using FormulaFlow.Server.Intermediate.Model.Card.Base;
using FormulaFlow.Server.Intermediate.Model.Enum;
using FormulaFlow.Server.Intermediate.Model.Parameter;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;

namespace FormulaFlow.Server.Intermediate.Model.Card
{
    public class NumberIntermediateCard : IntermediateFormulaCard
    {
        public override NetworkCardType Type => NetworkCardType.Number;

        public override bool MultiInput => true;

        public override CardIoDataType Input => CardIoDataType.Number;

        public override CardIoDataType Output => CardIoDataType.Number;

        public override string Label => "Number Formula";

        public override string Description => "Create formulas to transform the numeric input to the output";

        public override string DefaultName => "Number Formula";

        protected override IntermediateParameter[] _defaultParameters => [
                new NumberFormulaIntermediateParameter(0),
            ];

        public override async Task<Dictionary<int, int>> GetBuffer()
        {
            var param = (Parameters[0] as NumberFormulaIntermediateParameter);

            if (param == null)
            {
                throw new ArgumentException();
            }

            var builder = new FormulaRegexPatternBuilder();

            builder.IncludeFormulas(param.ContainerOperators);
            builder.IncludeFormulas(param.VariableOperators);
            builder.IncludeFormulas(param.NumericUnary);
            builder.IncludeFormulas(param.NumericBinary);

            var parser = builder.Build();

            _formula = parser.CompileFormula(param.Value, out var buffer);

            return buffer;
        }
    }
}
