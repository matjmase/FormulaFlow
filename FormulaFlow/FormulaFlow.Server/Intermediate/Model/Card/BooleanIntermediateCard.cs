using FormulaFlow.Data.Enum;
using FormulaFlow.Server.Intermediate.Formula;
using FormulaFlow.Server.Intermediate.Model.Card.Base;
using FormulaFlow.Server.Intermediate.Model.Enum;
using FormulaFlow.Server.Intermediate.Model.Parameter;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;

namespace FormulaFlow.Server.Intermediate.Model.Card
{
    public class BooleanIntermediateCard : IntermediateFormulaCard
    {
        public override NetworkCardType Type => NetworkCardType.Boolean;

        public override bool MultiInput => true;

        public override CardIoDataType Input => CardIoDataType.Boolean;

        public override CardIoDataType Output => CardIoDataType.Boolean;

        public override string Label => "Boolean Formula";

        public override string Description => "Boolean Formula";

        public override string DefaultName => "Boolean Formula";

        protected override IntermediateParameter[] _defaultParameters => [
                new BooleanFormulaIntermediateParameter(0),
            ];

        public override async Task<Dictionary<int, int>> GetBuffer()
        {
            var param = (Parameters[0] as BooleanFormulaIntermediateParameter);

            if (param == null)
            {
                throw new ArgumentException();
            }

            var builder = new FormulaRegexPatternBuilder();

            builder.IncludeFormulas(param.ContainerOperators);
            builder.IncludeFormulas(param.VariableOperators);
            builder.IncludeFormulas(param.BooleanUnary);
            builder.IncludeFormulas(param.BooleanBinary);

            var parser = builder.Build();

            _formula = parser.CompileFormula(param.Value, out var buffer);

            return buffer;
        }
    }
}
