using FormulaFlow.Server.Intermediate.Dto;
using FormulaFlow.Server.Intermediate.Factory;
using FormulaFlow.Server.Intermediate.Model.Card.Base;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;
using FormulaFlow.Server.Mapper.Base;

namespace FormulaFlow.Server.Service
{
    public class CardCatalogService : ICardCatalogService
    {
        private readonly IMapper<IntermediateCard, StockCardDto> _cardMapper;
        private readonly IMapper<IntermediateParameter, StockParameterDto> _paramMapper;

        public CardCatalogService(
                IMapper<IntermediateCard, StockCardDto> cardMapper,
                IMapper<IntermediateParameter, StockParameterDto> paramMapper
            )
        {
            _cardMapper = cardMapper;
            _paramMapper = paramMapper;
        }

        public Task<IEnumerable<StockCardDto>> Get()
        {
            return Task.FromResult(IntermediateCardFactory.CreateAllIntermediateCards().Select(card =>
            {
                var dtoCard = _cardMapper.Map(card);

                if (card.Parameters != null)
                {
                    var dtoParams = card.Parameters.Select(para => _paramMapper.Map(para));

                    dtoCard.Parameters = dtoParams;
                }

                return dtoCard;
            }));
        }
    }
}
