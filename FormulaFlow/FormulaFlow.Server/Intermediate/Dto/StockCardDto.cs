using FormulaFlow.Data.Enum;
using FormulaFlow.Server.Dto.Base;
using FormulaFlow.Server.Intermediate.Model.Enum;

namespace FormulaFlow.Server.Intermediate.Dto
{
    public class StockCardDto : BaseIdDtoModel
    {
        public Guid CanvasId { get; set; }

        public string Label { get; set; }
        public string Description { get; set; }

        public string DefaultName { get; set; }
        public string Name { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }

        public NetworkCardType Type { get; set; }
        public bool MultiInput { get; set; }
        public CardIoDataType Input { get; set; }
        public CardIoDataType Output { get; set; }

        // linking Response
        public IEnumerable<OrderedLinkDto>? PointsFromCards { get; set; }

        // linking Request
        public int NaiveId { get; set; }
        public IEnumerable<OrderedLinkNaiveDto>? NaivePointsToCardNaiveId { get; set; }

        // children
        public IEnumerable<StockParameterDto> Parameters { get; set; }
    }
}
