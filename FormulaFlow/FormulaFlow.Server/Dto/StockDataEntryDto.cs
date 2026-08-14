using FormulaFlow.Server.Dto.Base;

namespace FormulaFlow.Server.Dto
{
    public class StockDataEntryDto : NoSqlBaseIdDtoModel
    {
        public Guid StockSymbolId { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
    }
}
