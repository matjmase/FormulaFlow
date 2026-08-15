using FormulaFlow.Data.Enum;
using FormulaFlow.Server.Intermediate.Model.Card;
using FormulaFlow.Server.Intermediate.Model.Card.Base;

namespace FormulaFlow.Server.Intermediate.Factory
{
    public class IntermediateCardFactory
    {

        public static IntermediateCard CreateIntermediateCard(NetworkCardType type)
        {
            switch (type)
            {
                case NetworkCardType.DataSource:
                    return new DataSourceIntermediateCard();
                case NetworkCardType.Aggregate:
                    return new AggregateIntermediateCard();
                case NetworkCardType.Number:
                    return new NumberIntermediateCard();
                case NetworkCardType.Boolean:
                    return new BooleanIntermediateCard();
                case NetworkCardType.FeedbackNumber:
                    return new NumberFeedbackIntermediateCard();    
                case NetworkCardType.FeedbackBoolean:
                    return new BooleanFeedbackIntermediateCard();
                case NetworkCardType.Transitional:
                    return new TransitionalIntermediateCard();  
                default:
                    throw new NotImplementedException();
            }
        }

        public static IntermediateCard[] CreateAllIntermediateCards()
        {
            return new IntermediateCard[]
            {
                new DataSourceIntermediateCard(),
                new AggregateIntermediateCard(),
                new NumberIntermediateCard(),
                new BooleanIntermediateCard(), 
                new NumberFeedbackIntermediateCard(),
                new BooleanFeedbackIntermediateCard(),
                new TransitionalIntermediateCard(),
            };
        }
    }
}
