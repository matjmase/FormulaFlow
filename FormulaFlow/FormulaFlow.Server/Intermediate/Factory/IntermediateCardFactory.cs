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
                case NetworkCardType.FeedbackNumber:
                    return new NumberFeedbackIntermediateCard();    
                default:
                    throw new NotImplementedException();
            }
        }

        public static IntermediateCard[] CreateAllIntermediateCards()
        {
            return new IntermediateCard[]
            {
                new DataSourceIntermediateCard(),   
                new NumberFeedbackIntermediateCard(),
            };
        }
    }
}
