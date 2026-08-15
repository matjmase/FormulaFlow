namespace FormulaFlow.Server.Intermediate.Formula.Model.Computable
{
    public class VariableComputable : IComputable
    {
        public Func<IndexShiftArrayHolder, dynamic> SelectorFunction;

        public dynamic Compute(IndexShiftArrayHolder previous, dynamic feedback)
        {
            return SelectorFunction(previous);
        }
    }
}
