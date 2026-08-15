using FormulaFlow.Data.Enum;
using FormulaFlow.Server.Intermediate.Formula;
using FormulaFlow.Server.Intermediate.Model.Card.Base;
using FormulaFlow.Server.Intermediate.Model.Enum;
using FormulaFlow.Server.Intermediate.Model.Parameter;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;

namespace FormulaFlow.Server.Intermediate.Model.Card
{
    public class TransitionalIntermediateCard : IntermediateFormulaCard
    {
        public override NetworkCardType Type => NetworkCardType.Transitional;

        public override bool MultiInput => true;

        public override CardIoDataType Input => CardIoDataType.Number;

        public override CardIoDataType Output => CardIoDataType.Boolean;

        public override string Label => "Number to Bool Transition";

        public override string Description => "Number to Bool Transition";

        public override string DefaultName => "Transitional Formula";

        protected override IntermediateParameter[] _defaultParameters => [
                new TransitionFormulaIntermediateParameter(0),
            ];

        public override async Task<Dictionary<int, int>> GetBuffer()
        {
            var param = (Parameters[0] as TransitionFormulaIntermediateParameter);

            if (param == null)
            {
                throw new ArgumentException();
            }

            var builder = new FormulaRegexPatternBuilder();

            builder.IncludeFormulas(param.ContainerOperators);
            builder.IncludeFormulas(param.VariableOperators);
            builder.IncludeFormulas(param.NumericUnary);
            builder.IncludeFormulas(param.BooleanUnary);
            builder.IncludeFormulas(param.TransBinary);

            var parser = builder.Build();

            _formula = parser.CompileFormula(param.Value, out var buffer);

            return buffer;
        }
    }
}
