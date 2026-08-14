using FormulaFlow.Data.Models;
using FormulaFlow.Server.Repository.Base;

namespace FormulaFlow.Server.UnitOfWork
{
    public interface ICanvasUnitOfWork
    {
        IRepository<NetworkCanvas> Canvases { get; }
        IRepository<NetworkCard> Cards { get; }
        IRepository<NetworkCardToNetworkCard> CardToCards { get; }
        IRepository<NetworkParameter> Parameters { get; }
        Task<int> SaveChangesAsync();
    }
}
