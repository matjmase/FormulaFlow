using FormulaFlow.Data.Enum;
using FormulaFlow.Data.NoSql.Models;
using FormulaFlow.Server.Intermediate.Dto;
using FormulaFlow.Server.Intermediate.Model.Base;
using FormulaFlow.Server.Intermediate.Model.Enum;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;
using FormulaFlow.Server.NoSql.Repository.Base;

namespace FormulaFlow.Server.Intermediate.Model.Card.Base
{
    public abstract class IntermediateCard : BaseIntermediateModel
    {
        public Guid NetworkCanvasId { get; set; }

        private IntermediateParameter[] _parameters = new IntermediateParameter[0];

        protected abstract IntermediateParameter[] _defaultParameters { get; }

        public abstract string DefaultName { get; }
        public abstract NetworkCardType Type { get; }
        public abstract bool MultiInput { get; }
        public abstract CardIoDataType Input { get; }
        public abstract CardIoDataType Output { get; }
        public abstract string Label { get; }
        public abstract string Description { get; }


        public IntermediateParameter[] Parameters
        {
            get => _parameters;
            set => _parameters = value;
        }

        public Guid? Id { get; set; }
        public string Name { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }

        public IntermediateCard()
        {
            _parameters = _defaultParameters;
        }

        public abstract Task<Dictionary<int, int>> GetBuffer();

        public abstract Task<BackTestResultDto[]> Process(BackTestResultDto[][] inputs, INoSqlRepository<StockDataEntry> dataRepo, DateTime start, DateTime end, int totalBuffer);
    }
}
