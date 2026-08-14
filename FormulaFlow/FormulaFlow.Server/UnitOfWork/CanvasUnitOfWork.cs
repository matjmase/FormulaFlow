using FormulaFlow.Data;
using FormulaFlow.Data.Models;
using FormulaFlow.Server.Repository.Base;

namespace FormulaFlow.Server.UnitOfWork
{
    public class CanvasUnitOfWork : ICanvasUnitOfWork
    {
        private readonly FormulaFlowContext _context;

        private readonly IRepository<NetworkCanvas> _canvases;
        private readonly IRepository<NetworkCard> _cards;
        private readonly IRepository<NetworkCardToNetworkCard> _cardToCards;
        private readonly IRepository<NetworkParameter> _parameters;

        public IRepository<NetworkCanvas> Canvases => _canvases;

        public IRepository<NetworkCard> Cards => _cards;

        public IRepository<NetworkCardToNetworkCard> CardToCards => _cardToCards;

        public IRepository<NetworkParameter> Parameters => _parameters;

        public CanvasUnitOfWork(
                FormulaFlowContext context,
                IRepository<NetworkCanvas> canvases,
                IRepository<NetworkCard> cards,
                IRepository<NetworkCardToNetworkCard> cardToCards,
                IRepository<NetworkParameter> parameters
            )
        {
            _context = context;

            _canvases = canvases;
            _cards = cards;
            _cardToCards = cardToCards;
            _parameters = parameters;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
