using FormulaFlow.Data.Models;
using FormulaFlow.Server.Intermediate.Model.Canvas.Base;
using FormulaFlow.Server.Mapper.Base;

namespace FormulaFlow.Server.Intermediate.Mapper.Database
{
    public class DatabaseIntermediateCanvasMapper : IMapper<NetworkCanvas, IntermediateCanvas>, IMapper<IntermediateCanvas, NetworkCanvas>
    {
        public IntermediateCanvas Map(NetworkCanvas from)
        {
            return new IntermediateCanvas
            {
                Id = from.Id,
                Name = from.Name,
                Scale = from.Scale,
                Height = from.Height,
                Width = from.Width,
            };
        }

        public NetworkCanvas Map(IntermediateCanvas from)
        {
            return new NetworkCanvas
            {
                Id = from.Id ?? new Guid(),
                Name = from.Name,
                Scale = from.Scale,
                Height = from.Height,
                Width = from.Width,
            };
        }
    }
}
