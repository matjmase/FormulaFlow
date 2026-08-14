using FormulaFlow.Data.Models;
using FormulaFlow.Server.Intermediate.Factory;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;
using FormulaFlow.Server.Mapper.Base;

namespace FormulaFlow.Server.Intermediate.Mapper.Database
{
    public class DatabaseIntermediateParameterMapper : IMapper<NetworkParameter, IntermediateParameter>, IMapper<IntermediateParameter, NetworkParameter>
    {
        public IntermediateParameter Map(NetworkParameter from)
        {
            var retVal = IntermediateParameterFactory.CreateIntermediateParameter(from.Type, from.Order);

            retVal.Id = from.Id;
            retVal.NetworkCardId = from.NetworkCardId;
            retVal.Value = from.Value;

            return retVal;
        }

        public NetworkParameter Map(IntermediateParameter from)
        {
            return new NetworkParameter
            {
                Id = from.Id ?? new Guid(),
                NetworkCardId = from.NetworkCardId,
                Order = from.Order,
                Value = from.Value,
                Type = from.Type,
            };
        }
    }
}
