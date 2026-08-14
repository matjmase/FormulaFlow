using FormulaFlow.Data.Models;
using FormulaFlow.Server.Dto;
using FormulaFlow.Server.Mapper.Base;

namespace FormulaFlow.Server.Mapper
{
    public class StockCanvasSimpleMapper : IMapper<NetworkCanvas, StockCanvasSimpleDto>, IMapper<StockCanvasSimpleDto, NetworkCanvas>
    {
        public StockCanvasSimpleDto Map(NetworkCanvas from)
        {
            return new StockCanvasSimpleDto
            {
                Id = from.Id,
                Name = from.Name,
                Scale = from.Scale,
                Height = from.Height,
                Width = from.Width,
                CreatedByUserId = from.CreatedByUserId,
                UpdatedByUserId = from.UpdatedByUserId
            };
        }

        public NetworkCanvas Map(StockCanvasSimpleDto from)
        {
            throw new NotImplementedException();
        }
    }
}
