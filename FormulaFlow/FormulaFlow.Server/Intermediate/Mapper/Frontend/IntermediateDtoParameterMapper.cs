using FormulaFlow.Server.Intermediate.Dto;
using FormulaFlow.Server.Intermediate.Factory;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;
using FormulaFlow.Server.Mapper.Base;

namespace FormulaFlow.Server.Intermediate.Mapper.Frontend
{
    public class IntermediateDtoParameterMapper : IMapper<IntermediateParameter, StockParameterDto>, IMapper<StockParameterDto, IntermediateParameter>
    {
        public StockParameterDto Map(IntermediateParameter from)
        {
            return new StockParameterDto
            {
                Id = from.Id,
                Order = from.Order,
                Type = from.Type,
                Description = from.Description,
                ToolTip = from.ToolTip,
                Value = from.Value,
            };
        }

        public IntermediateParameter Map(StockParameterDto from)
        {
            var retVal = IntermediateParameterFactory.CreateIntermediateParameter(from.Type, from.Order);

            retVal.Id = from.Id;
            retVal.NetworkCardId = from.CardId;
            retVal.Value = from.Value;

            return retVal;
        }
    }
}
