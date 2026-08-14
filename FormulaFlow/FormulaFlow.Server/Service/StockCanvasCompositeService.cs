using FormulaFlow.Data.Models;
using FormulaFlow.Data.Models.Base;
using FormulaFlow.Server.Intermediate.Dto;
using FormulaFlow.Server.Intermediate.Model.Canvas.Base;
using FormulaFlow.Server.Intermediate.Model.Card.Base;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;
using FormulaFlow.Server.Mapper.Base;
using FormulaFlow.Server.Repository.Base;
using FormulaFlow.Server.UnitOfWork;

namespace FormulaFlow.Server.Service
{
    public class StockCanvasCompositeService : IStockCanvasCompositeService
    {
        private readonly ICanvasUnitOfWork _unitOfWork;

        private readonly IMapper<NetworkCanvas, IntermediateCanvas> _canvasMapperOut1;
        private readonly IMapper<IntermediateCanvas, NetworkCanvas> _canvasMapperIn2;
        private readonly IMapper<NetworkCard, IntermediateCard> _cardMapperOut1;
        private readonly IMapper<IntermediateCard, NetworkCard> _cardMapperIn2;
        private readonly IMapper<NetworkParameter, IntermediateParameter> _parameterMapperOut1;
        private readonly IMapper<IntermediateParameter, NetworkParameter> _parameterMapperIn2;

        private readonly IMapper<IntermediateCanvas, StockCanvasDto> _canvasMapperOut2;
        private readonly IMapper<StockCanvasDto, IntermediateCanvas> _canvasMapperIn1;
        private readonly IMapper<IntermediateCard, StockCardDto> _cardMapperOut2;
        private readonly IMapper<StockCardDto, IntermediateCard> _cardMapperIn1;
        private readonly IMapper<IntermediateParameter, StockParameterDto> _parameterMapperOut2;
        private readonly IMapper<StockParameterDto, IntermediateParameter> _parameterMapperIn1;

        public StockCanvasCompositeService(
                ICanvasUnitOfWork unitOfWork,
                IMapper<NetworkCanvas, IntermediateCanvas> canvasMapperOut1,
                IMapper<IntermediateCanvas, NetworkCanvas> canvasMapperIn2,
                IMapper<NetworkCard, IntermediateCard> cardMapperOut1,
                IMapper<IntermediateCard, NetworkCard> cardMapperIn2,
                IMapper<NetworkParameter, IntermediateParameter> parameterMapperOut1,
                IMapper<IntermediateParameter, NetworkParameter> parameterMapperIn2,
                IMapper<IntermediateCanvas, StockCanvasDto> canvasMapperOut2,
                IMapper<StockCanvasDto, IntermediateCanvas> canvasMapperIn1,
                IMapper<IntermediateCard, StockCardDto> cardMapperOut2,
                IMapper<StockCardDto, IntermediateCard> cardMapperIn1,
                IMapper<IntermediateParameter, StockParameterDto> parameterMapperOut2,
                IMapper<StockParameterDto, IntermediateParameter> parameterMapperIn1
            )
        {
            _unitOfWork = unitOfWork;
            _canvasMapperOut1 = canvasMapperOut1;
            _canvasMapperIn2 = canvasMapperIn2;
            _cardMapperOut1 = cardMapperOut1;
            _cardMapperIn2 = cardMapperIn2;
            _parameterMapperOut1 = parameterMapperOut1;
            _parameterMapperIn2 = parameterMapperIn2;
            _canvasMapperOut2 = canvasMapperOut2;
            _canvasMapperIn1 = canvasMapperIn1;
            _cardMapperOut2 = cardMapperOut2;
            _cardMapperIn1 = cardMapperIn1;
            _parameterMapperOut2 = parameterMapperOut2;
            _parameterMapperIn1 = parameterMapperIn1;
        }

        public async Task<StockCanvasDto> Add(StockCanvasDto addDto, string userId)
        {
            return await AddOrUpdateCanvas(addDto, userId);
        }

        public async Task<StockCanvasDto> Get(Guid id)
        {
            return await GetComplexItem(id);
        }

        public async Task<StockCanvasDto> Update(StockCanvasDto updateDto, string userId)
        {
            return await AddOrUpdateCanvas(updateDto, userId);
        }

        private async Task<StockCanvasDto> GetComplexItem(Guid canvasId)
        {
            var canvas = await _unitOfWork.Canvases.GetByIdAsync(canvasId);
            var canvasDto = _canvasMapperOut2.Map(_canvasMapperOut1.Map(canvas));

            var cards = await _unitOfWork.Cards.GetAllAsync(card => card.NetworkCanvasId == canvasId);
            var cardDtos = cards.Select(card => _cardMapperOut2.Map(_cardMapperOut1.Map(card))).ToArray();
            canvasDto.Cards = cardDtos;

            for (var i = 0; i < cardDtos.Length; i++)
            {
                var cardDto = cardDtos[i];

                var links = await _unitOfWork.CardToCards.GetAllAsync(ctc => ctc.To == cardDto.Id);
                cardDto.PointsFromCards = links.Select(link => new OrderedLinkDto
                {
                    Id = link.Id,
                    From = link.From,
                    Order = link.Order,
                });

                var parameters = await _unitOfWork.Parameters.GetAllAsync(para => para.NetworkCardId == cardDto.Id);
                cardDto.Parameters = parameters.Select(para => _parameterMapperOut2.Map(_parameterMapperOut1.Map(para)));
            }

            return canvasDto;
        }


        private async Task<StockCanvasDto> AddOrUpdateCanvas(StockCanvasDto clientDto, string userId)
        {
            var dbCanvas = _canvasMapperIn2.Map(_canvasMapperIn1.Map(clientDto));

            var canvasId = (await AddOrUpdateToRepo(dbCanvas, userId, _unitOfWork.Canvases)).Id;

            var dtoCards = clientDto.Cards;

            var cardIdMap = new Dictionary<int, Guid>();
            var cardIds = new HashSet<Guid>();

            foreach (var card in dtoCards)
            {
                var dbCard = _cardMapperIn2.Map(_cardMapperIn1.Map(card));

                dbCard.NetworkCanvasId = canvasId;

                var cardId = (await AddOrUpdateToRepo(dbCard, userId, _unitOfWork.Cards)).Id;

                cardIdMap.Add(card.NaiveId, cardId);
                cardIds.Add(cardId);

                var dtoParameters = card.Parameters;

                foreach (var param in dtoParameters)
                {
                    var dbParam = _parameterMapperIn2.Map(_parameterMapperIn1.Map(param));

                    dbParam.NetworkCardId = cardId;

                    await AddOrUpdateToRepo(dbParam, userId, _unitOfWork.Parameters);
                }
            }

            var totalCards = await _unitOfWork.Cards.GetAllAsync(card => card.NetworkCanvasId == dbCanvas.Id);
            var toRemoveCard = totalCards.Where(card => !cardIds.Contains(card.Id));

            foreach (var remove in toRemoveCard)
            {
                _unitOfWork.Cards.Delete(remove);
            }

            foreach (var card in dtoCards)
            {
                var currentId = cardIdMap[card.NaiveId];

                var newRelaHash = new HashSet<Guid>();
                var newOrderDictionary = new Dictionary<Guid, int>();

                if (card.NaivePointsToCardNaiveId != null)
                {
                    foreach (var pointTo in card.NaivePointsToCardNaiveId)
                    {
                        var other = cardIdMap[pointTo.Link];

                        newOrderDictionary.Add(other, pointTo.Order);

                        newRelaHash.Add(other);
                    }
                }

                var currentRelationships = await _unitOfWork.CardToCards.GetAllAsync(rela => rela.From == currentId);
                var currRelaHash = currentRelationships.Select(rela => rela.From).ToHashSet();

                var toRemove = currentRelationships.Where(rela => !newRelaHash.Contains(rela.From));
                var toAdd = newRelaHash.Where(point => !currRelaHash.Contains(point));

                foreach (var remove in toRemove)
                {
                    _unitOfWork.CardToCards.Delete(remove);
                }

                foreach (var add in toAdd)
                {
                    var newRelationship = new NetworkCardToNetworkCard
                    {
                        From = add,
                        To = currentId,
                        Order = newOrderDictionary[add]
                    };

                    await _unitOfWork.CardToCards.AddAsync(newRelationship, userId);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return await GetComplexItem(dbCanvas.Id);
        }

        private async Task<TDbElement> AddOrUpdateToRepo<TDbElement>(TDbElement entity, string userId, IRepository<TDbElement> repo)
            where TDbElement : BaseIdEntityModel
        {
            var retVal = default(TDbElement);

            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
                retVal = await repo.AddAsync(entity, userId);
            }
            else
            {
                retVal = await repo.Update(entity, userId);
            }

            return retVal;
        }
    }
}
