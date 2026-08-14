using FormulaFlow.Data.Enum;
using FormulaFlow.Server.Dto.Base;

namespace FormulaFlow.Server.Intermediate.Dto
{
    public class StockParameterDto : BaseIdDtoModel
    {
        public Guid CardId { get; set; }
        public int Order { get; set; }
        public NetworkParameterType Type { get; set; }
        public string Description { get; set; }
        public string? ToolTip { get; set; }
        public string Value { get; set; }
    }
}
