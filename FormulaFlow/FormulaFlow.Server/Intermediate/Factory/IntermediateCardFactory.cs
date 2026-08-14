using FormulaFlow.Data.Enum;
using FormulaFlow.Server.Intermediate.Model.Card.Base;

namespace FormulaFlow.Server.Intermediate.Factory
{
    public class IntermediateCardFactory
    {

        public static IntermediateCard CreateIntermediateCard(NetworkCardType type)
        {
            switch (type)
            {
                default:
                    throw new NotImplementedException();
            }
        }

        public static IntermediateCard[] CreateAllIntermediateCards()
        {
            return new IntermediateCard[]
            {
            };
        }
    }
}
