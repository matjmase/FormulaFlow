using FormulaFlow.Data.Models;
using FormulaFlow.Server.Intermediate.Factory;
using FormulaFlow.Server.Intermediate.Model.Card.Base;
using FormulaFlow.Server.Mapper.Base;

namespace FormulaFlow.Server.Intermediate.Mapper.Database
{
    public class DatabaseIntermediateCardMapper : IMapper<NetworkCard, IntermediateCard>, IMapper<IntermediateCard, NetworkCard>
    {
        public IntermediateCard Map(NetworkCard from)
        {
            var retVal = IntermediateCardFactory.CreateIntermediateCard(from.NetworkType);

            retVal.Id = from.Id;
            retVal.NetworkCanvasId = from.NetworkCanvasId;
            retVal.Name = from.Name;
            retVal.Top = from.Top;
            retVal.Left = from.Left;

            return retVal;
        }

        public NetworkCard Map(IntermediateCard entity)
        {
            return new NetworkCard
            {
                Id = entity.Id ?? new Guid(),
                NetworkCanvasId = entity.NetworkCanvasId,
                Name = entity.Name,
                Top = entity.Top,
                Left = entity.Left,
                NetworkType = entity.Type,
            };
        }
    }
}
