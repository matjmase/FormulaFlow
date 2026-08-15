using FormulaFlow.Data.NoSql.Models;
using FormulaFlow.Server.Dto;
using FormulaFlow.Server.Intermediate.Dto;
using FormulaFlow.Server.Intermediate.Formula.Model;
using FormulaFlow.Server.Intermediate.Formula.Model.Computable;
using FormulaFlow.Server.NoSql.Repository.Base;

namespace FormulaFlow.Server.Intermediate.Model.Card.Base
{
    public abstract class IntermediateFormulaCard : IntermediateCard
    {
        protected IComputable _formula;

        public override async Task<BackTestResultDto[]> Process(BackTestResultDto[][] inputs, INoSqlRepository<StockDataEntry> dataRepo, DateTime start, DateTime end, int totalBuffer)
        {
            var buffers = await GetBuffer();

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

            var output = new BackTestResultDto[totalLength.First()];

            for (var i = 0; i < output.Length; i++)
            {
                var input = new IndexShiftArrayHolder(inputs, buffers, i);

                var date = GetDate(inputs, buffers, i);

                output[i] = new BackTestResultDto { Date = date, Value = _formula.Compute(input, 0) };
            }

            return output;
        }

        protected DateTime GetDate(BackTestResultDto[][] inputs, Dictionary<int, int> dimShift, int indexShift)
        {
            DateTime? date = null;

            // get date
            for (var i = 0; i < inputs.Length; i++)
            {
                if (dimShift.ContainsKey(i))
                {
                    var buffer = dimShift[i];

                    var totalIndex = indexShift + buffer;

                    if (inputs[i].Length != 0)
                    {
                        if (date == null)
                        {
                            date = inputs[i][totalIndex].Date;
                        }
                        else if (date != inputs[i][totalIndex].Date)
                        {
                            throw new Exception("Date mismatch between inputs");
                        }
                    }
                }
                else
                {
                    if (inputs[i].Length != 0)
                    {
                        if (date == null)
                        {
                            date = inputs[i][indexShift].Date;
                        }
                        else if (date != inputs[i][indexShift].Date)
                        {
                            throw new Exception("Date mismatch between inputs");
                        }
                    }
                }
            }

            return (DateTime)date;
        }
    }
}
