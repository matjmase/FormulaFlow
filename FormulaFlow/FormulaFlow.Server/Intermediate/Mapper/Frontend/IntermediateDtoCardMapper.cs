using FormulaFlow.Server.Intermediate.Dto;
using FormulaFlow.Server.Intermediate.Factory;
using FormulaFlow.Server.Intermediate.Model.Card.Base;
using FormulaFlow.Server.Mapper.Base;

namespace FormulaFlow.Server.Intermediate.Mapper.Frontend
{
    public class IntermediateDtoCardMapper : IMapper<IntermediateCard, StockCardDto>, IMapper<StockCardDto, IntermediateCard>
    {
        public StockCardDto Map(IntermediateCard from)
        {
            return new StockCardDto
            {
                Id = from.Id,
                CanvasId = from.NetworkCanvasId,

                Label = from.Label,
                Description = from.Description,

                DefaultName = from.DefaultName,
                Name = from.Name,
                Top = from.Top,
                Left = from.Left,

                Type = from.Type,
                MultiInput = from.MultiInput,
                Input = from.Input,
                Output = from.Output,
            };
        }

        public IntermediateCard Map(StockCardDto from)
        {
            var retVal = IntermediateCardFactory.CreateIntermediateCard(from.Type);

            retVal.Id = from.Id;
            retVal.NetworkCanvasId = from.CanvasId;

            retVal.Name = from.Name;
            retVal.Top = from.Top;
            retVal.Left = from.Left;

            return retVal;
        }
    }
}
