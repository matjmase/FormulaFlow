using FormulaFlow.Data.Enum;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;

namespace FormulaFlow.Server.Intermediate.Factory
{
    public class IntermediateParameterFactory
    {
        public static IntermediateParameter CreateIntermediateParameter(NetworkParameterType type, int order)
        {
            switch (type)
            {
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
