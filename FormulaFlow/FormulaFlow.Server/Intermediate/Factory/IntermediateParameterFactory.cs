using FormulaFlow.Data.Enum;
using FormulaFlow.Data.Models;
using FormulaFlow.Server.Intermediate.Model.Parameter;
using FormulaFlow.Server.Intermediate.Model.Parameter.Base;

namespace FormulaFlow.Server.Intermediate.Factory
{
    public class IntermediateParameterFactory
    {
        public static IntermediateParameter CreateIntermediateParameter(NetworkParameterType type, int order)
        {
            switch (type)
            {
                case NetworkParameterType.StockSource:
                    return new StockSymbolIntermediateParameter(order);
                case NetworkParameterType.BufferInput:
                    return new BufferInputIntermediateParameter(order);
                case NetworkParameterType.NumericInput:
                    return new NumericInputIntermediateParameter(order);
                case NetworkParameterType.Boolean:
                    return new BooleanFormulaIntermediateParameter(order);
                case NetworkParameterType.Number:
                    return new NumberFormulaIntermediateParameter(order);       
                case NetworkParameterType.NumberFeedback:
                    return new NumberFeedbackFormulaIntermediateParameter(order);
                case NetworkParameterType.BooleanFeedback:
                    return new BooleanFeedbackFormulaIntermediateParameter(order);      
                case NetworkParameterType.AggregateMethod:
                    return new AggregateMethodIntermediateParameter(order);     
                case NetworkParameterType.Transitional:
                    return new TransitionFormulaIntermediateParameter(order);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
