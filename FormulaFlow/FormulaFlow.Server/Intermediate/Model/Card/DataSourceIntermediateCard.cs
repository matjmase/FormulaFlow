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
    public class DataSourceIntermediateCard : IntermediateCard
    {
        public override NetworkCardType Type => NetworkCardType.DataSource;

        public override bool MultiInput => false;

        public override CardIoDataType Input => CardIoDataType.None;

        public override CardIoDataType Output => CardIoDataType.Number;

        public override string Label => "Data Source";

        public override string Description => "Stock data source";

        public override string DefaultName => "Data Source";

        protected override IntermediateParameter[] _defaultParameters => [
                new StockSymbolIntermediateParameter(0),
            ];

        private TimeSpan _bufferDuration = TimeSpan.FromDays(30);

        public override async Task<Dictionary<int, int>> GetBuffer()
        {


            var buffer = new Dictionary<int, int>();

            return buffer;
        }

        public override async Task<BackTestResultDto[]> Process(BackTestResultDto[][] inputs, INoSqlRepository<StockDataEntry> dataRepo, DateTime start, DateTime end, int totalBuffer)
        {
            var param = (Parameters[0] as StockSymbolIntermediateParameter);

            if (param == null)
            {
                throw new ArgumentException();
            }

            var stockSymbolGuid = new Guid(param.Value);

            var data = await dataRepo.GetAllAsync(data => data.StockSymbolId == stockSymbolGuid && data.Date >= start && data.Date <= end);

            if (totalBuffer > 0)
            {
                IEnumerable<StockDataEntry> bucket;

                var bufferStart = start;

                while (totalBuffer > 0)
                {
                    var bufferEnd = bufferStart;
                    bufferStart = bufferStart.Add(-_bufferDuration);

                    bucket = (await dataRepo.GetAllAsync(data => data.StockSymbolId == stockSymbolGuid && data.Date >= bufferStart && data.Date < bufferEnd)).OrderByDescending(data => data.Date).Take(totalBuffer);

                    if (!bucket.Any())
                    {
                        throw new ArgumentException();
                    }

                    data = data.Union(bucket);
                    totalBuffer -= bucket.Count();
                }
            }

            return data.OrderBy(data => data.Date).Select(data => new BackTestResultDto { Date = data.Date, Value = data.Amount }).ToArray();
        }
    }
}
