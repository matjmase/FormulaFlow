using FormulaFlow.Data.Models;
using FormulaFlow.Data.NoSql.Models;
using FormulaFlow.Server.Intermediate.Dto;
using FormulaFlow.Server.Intermediate.Model.Card.Base;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;
using FormulaFlow.Server.Mapper.Base;
using FormulaFlow.Server.NoSql.Repository.Base;
using FormulaFlow.Server.Repository.Base;

namespace FormulaFlow.Server.Service
{
    public class BackTestService : IBackTestService
    {
        private readonly IRepository<NetworkCard> _cardRepo;
        private readonly IRepository<NetworkParameter> _paramRepo;
        private readonly IRepository<NetworkCardToNetworkCard> _cardToCardRepo;
        private readonly INoSqlRepository<StockDataEntry> _dataRepo;
        private readonly IMapper<NetworkCard, IntermediateCard> _cardMapper;
        private readonly IMapper<NetworkParameter, IntermediateParameter> _parameterMapper;

        public BackTestService(
                IRepository<NetworkCard> cardRepo,
                IRepository<NetworkParameter> paramRepo,
                IRepository<NetworkCardToNetworkCard> cardToCardRepo,
                INoSqlRepository<StockDataEntry> dataRepo,
                IMapper<NetworkCard, IntermediateCard> cardMapper,
                IMapper<NetworkParameter, IntermediateParameter> parameterMapper
            )
        {
            _cardRepo = cardRepo;
            _paramRepo = paramRepo;
            _cardToCardRepo = cardToCardRepo;
            _dataRepo = dataRepo;
            _cardMapper = cardMapper;
            _parameterMapper = parameterMapper;
        }

        public async Task<IEnumerable<BackTestResultDto>> Get(Guid cardGuid, DateTime start, DateTime end)
        {
            var backwards = new Dictionary<IntermediateCard, Dictionary<IntermediateCard, int>>();

            var explore = new Queue<IntermediateCard>();

            var first = await GetCard(cardGuid);

            explore.Enqueue(first);

            while (explore.Count > 0)
            {
                var focus = explore.Dequeue();

                if (backwards.ContainsKey(focus))
                {
                    throw new ArgumentException();
                }

                backwards.Add(focus, new Dictionary<IntermediateCard, int>());

                var links = await _cardToCardRepo.GetAllAsync(ctc => ctc.To == focus.Id);

                foreach (var link in links)
                {
                    var cardLink = await GetCard(link.From);

                    explore.Enqueue(cardLink);
                    backwards[focus].Add(cardLink, link.Order);
                }
            }

            var bufferDict = new Dictionary<IntermediateCard, Dictionary<IntermediateCard, int>>();

            await CalculateBuffers(backwards, first, bufferDict);

            var output = await ProcessRecursive(backwards, first, start, end, 0, bufferDict);

            return output;
        }

        private async Task CalculateBuffers(Dictionary<IntermediateCard, Dictionary<IntermediateCard, int>> backwards, IntermediateCard focus, Dictionary<IntermediateCard, Dictionary<IntermediateCard, int>> bufferDict)
        {
            var keys = backwards[focus].Keys.ToArray();
            var children = backwards[focus];

            var cardBuffer = new Dictionary<IntermediateCard, int>();

            bufferDict.Add(focus, cardBuffer);

            var buffers = await focus.GetBuffer();

            foreach (var key in keys)
            {
                var order = backwards[focus][key];

                if (buffers.ContainsKey(order))
                {
                    cardBuffer.Add(key, buffers[order]);
                }

                await CalculateBuffers(backwards, key, bufferDict);
            }
        }

        private async Task<BackTestResultDto[]> ProcessRecursive(Dictionary<IntermediateCard, Dictionary<IntermediateCard, int>> backwards, IntermediateCard focus, DateTime start, DateTime end, int totalBuffer, Dictionary<IntermediateCard, Dictionary<IntermediateCard, int>> bufferDict)
        {
            var keys = backwards[focus].Keys.ToArray();
            var children = backwards[focus];

            var input = new BackTestResultDto[children.Any() ? children.Values.Max() + 1 : 0][];

            foreach (var key in keys)
            {
                var buffer = 0;

                if (bufferDict[focus].ContainsKey(key))
                {
                    buffer = bufferDict[focus][key];
                }

                input[children[key]] = await ProcessRecursive(backwards, key, start, end, buffer + totalBuffer, bufferDict);
            }

            return await focus.Process(input, _dataRepo, start, end, totalBuffer);
        }

        private async Task<IntermediateCard> GetCard(Guid cardGuid)
        {
            var card = _cardMapper.Map(await _cardRepo.GetByIdAsync(cardGuid));
            var parameters = await _paramRepo.GetAllAsync(para => para.NetworkCardId == cardGuid);
            card.Parameters = parameters.OrderBy(p => p.Order).Select(para => _parameterMapper.Map(para)).ToArray();

            return card;
        }
    }
}
