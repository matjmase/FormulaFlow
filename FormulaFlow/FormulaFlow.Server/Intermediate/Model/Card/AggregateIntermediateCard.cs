using FormulaFlow.Data.Enum;
using FormulaFlow.Data.NoSql.Models;
using FormulaFlow.Server.Dto;
using FormulaFlow.Server.Intermediate.Dto;
using FormulaFlow.Server.Intermediate.Model.Card.Base;
using FormulaFlow.Server.Intermediate.Model.Enum;
using FormulaFlow.Server.Intermediate.Model.Parameter;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;
using FormulaFlow.Server.NoSql.Repository.Base;

namespace FormulaFlow.Server.Intermediate.Model.Card
{
    public class AggregateIntermediateCard : IntermediateCard
    {
        public override NetworkCardType Type => NetworkCardType.Aggregate;

        public override bool MultiInput => true;

        public override CardIoDataType Input => CardIoDataType.Number;

        public override CardIoDataType Output => CardIoDataType.Number;

        public override string Label => "Aggregate";

        public override string Description => "Aggregate";

        public override string DefaultName => "Aggregate";

        protected override IntermediateParameter[] _defaultParameters => [
                new AggregateMethodIntermediateParameter(0),
                new NumericInputIntermediateParameter(1),
            ];

        public override async Task<Dictionary<int, int>> GetBuffer()
        {
            var param = (Parameters[1] as NumericInputIntermediateParameter);

            if (param == null)
            {
                throw new ArgumentException();
            }

            var intVal = int.Parse(param.Value) - 1;

            var buffer = new Dictionary<int, int>();

            buffer.Add(0, intVal);

            return buffer;
        }

        public override async Task<BackTestResultDto[]> Process(BackTestResultDto[][] inputs, INoSqlRepository<StockDataEntry> dataRepo, DateTime start, DateTime end, int totalBuffer)
        {
            var methodParam = (Parameters[0] as AggregateMethodIntermediateParameter);

            AggregateMethodIntermediateParameterType methodType = (AggregateMethodIntermediateParameterType)System.Enum.Parse(typeof(AggregateMethodIntermediateParameterType), methodParam.Value);

            var buffer = (await GetBuffer()).First().Value;

            var totalLength = inputs.First().Length - buffer;

            var output = new BackTestResultDto[totalLength];

            switch (methodType)
            {
                case AggregateMethodIntermediateParameterType.Average:
                    for (var i = 0; i < output.Length; i++)
                    {
                        var sum = 0.0;

                        for (var j = 0; j <= buffer; j++)
                        {
                            sum += inputs[0][i + j].Value;
                        }

                        output[i] = new BackTestResultDto
                        {
                            Value = sum / (buffer + 1),
                            Date = inputs[0][i + buffer].Date
                        };
                    }
                    break;
                case AggregateMethodIntermediateParameterType.Summation:
                    for (var i = 0; i < output.Length; i++)
                    {
                        var sum = 0.0;

                        for (var j = 0; j <= buffer; j++)
                        {
                            sum += inputs[0][i + j].Value;
                        }

                        output[i] = new BackTestResultDto
                        {
                            Value = sum,
                            Date = inputs[0][i + buffer].Date
                        };
                    }
                    break;
                case AggregateMethodIntermediateParameterType.Multiplicative:

                    for (var i = 0; i < output.Length; i++)
                    {
                        var mult = 1.0;

                        for (var j = 0; j <= buffer; j++)
                        {
                            mult *= inputs[0][i + j].Value;
                        }

                        output[i] = new BackTestResultDto
                        {
                            Value = mult,
                            Date = inputs[0][i + buffer].Date
                        };
                    }
                    break;
                default: throw new NotImplementedException();
            }


            return output;
        }
    }
}
