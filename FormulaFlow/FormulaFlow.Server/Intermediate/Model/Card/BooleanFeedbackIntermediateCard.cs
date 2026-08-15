using FormulaFlow.Data.Enum;
using FormulaFlow.Data.NoSql.Models;
using FormulaFlow.Server.Dto;
using FormulaFlow.Server.Intermediate.Dto;
using FormulaFlow.Server.Intermediate.Formula;
using FormulaFlow.Server.Intermediate.Formula.Model;
using FormulaFlow.Server.Intermediate.Model.Card.Base;
using FormulaFlow.Server.Intermediate.Model.Enum;
using FormulaFlow.Server.Intermediate.Model.Parameter;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;
using FormulaFlow.Server.NoSql.Repository.Base;

namespace FormulaFlow.Server.Intermediate.Model.Card
{
    public class BooleanFeedbackIntermediateCard : IntermediateFormulaCard
    {
        public override NetworkCardType Type => NetworkCardType.FeedbackBoolean;

        public override bool MultiInput => true;

        public override CardIoDataType Input => CardIoDataType.Boolean;

        public override CardIoDataType Output => CardIoDataType.Boolean;

        public override string Label => "Boolean Feedback Formula";

        public override string Description => "Create formulas to transform the Boolean input to the output";

        public override string DefaultName => "Boolean Feedback Formula";

        protected override IntermediateParameter[] _defaultParameters => [
                new BooleanFeedbackFormulaIntermediateParameter(0),
                new BufferInputIntermediateParameter(1),
                new BooleanInputIntermediateParameter(2),
            ];

        public override async Task<Dictionary<int, int>> GetBuffer()
        {
            var param = (Parameters[0] as BooleanFeedbackFormulaIntermediateParameter);

            if (param == null)
            {
                throw new ArgumentException();
            }


            var univBufferString = (Parameters[1] as BufferInputIntermediateParameter)?.Value;

            if (univBufferString == null)
            {
                throw new ArgumentException();
            }

            var univBuffer = int.Parse(univBufferString);

            var builder = new FormulaRegexPatternBuilder();

            builder.IncludeFormulas(param.ContainerOperators);
            builder.IncludeFormulas(param.VariableOperators);
            builder.IncludeFormulas(param.VariableFeedbackOperators);
            builder.IncludeFormulas(param.BooleanUnary);
            builder.IncludeFormulas(param.BooleanBinary);

            var parser = builder.Build();

            _formula = parser.CompileFormula(param.Value, out var buffer);

            foreach (var key in buffer.Keys)
            {
                buffer[key] = buffer[key] + univBuffer;
            }

            return buffer;
        }

        public override async Task<BackTestResultDto[]> Process(BackTestResultDto[][] inputs, INoSqlRepository<StockDataEntry> dataRepo, DateTime start, DateTime end, int totalBuffer)
        {
            var univBufferString = (Parameters[1] as BufferInputIntermediateParameter)?.Value;

            if (univBufferString == null)
            {
                throw new ArgumentException();
            }

            var univBuffer = int.Parse(univBufferString);

            var param = (Parameters[2] as BooleanInputIntermediateParameter);

            if (param == null)
            {
                throw new ArgumentException();
            }

            var seed = bool.Parse(param.Value);

            var buffers = await GetBuffer();

            foreach (var key in buffers.Keys)
            {
                buffers[key] = buffers[key] - univBuffer;
            }

            var totalLength = new HashSet<int>();
            for (var i = 0; i < inputs.Length; i++)
            {
                var buffer = 0;

                if (buffers.ContainsKey(i))
                {
                    buffer = buffers[i];
                }

                totalLength.Add(inputs[i].Length - buffer);
            }

            if (totalLength.Count > 1)
            {
                throw new ArgumentException();
            }

            var resultLength = totalLength.First() - univBuffer;

            var output = new BackTestResultDto[resultLength];

            var lastValue = seed;

            for (var i = 0; i < univBuffer; i++)
            {
                var input = new IndexShiftArrayHolder(inputs, buffers, i);

                var date = GetDate(inputs, buffers, i);

                lastValue = _formula.Compute(input, lastValue);
            }

            for (var i = 0; i < output.Length; i++)
            {
                var input = new IndexShiftArrayHolder(inputs, buffers, i + univBuffer);

                var date = GetDate(inputs, buffers, i);

                lastValue = _formula.Compute(input, lastValue);
                output[i] = new BackTestResultDto { Date = date, Value = lastValue };
            }

            return output;
        }
    }
}
