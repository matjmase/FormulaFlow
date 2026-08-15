namespace FormulaFlow.Server.Intermediate.Formula.Model.Computable
{
    public class ConstantComputable : IComputable
    {
        public dynamic Value;

        public dynamic Compute(IndexShiftArrayHolder previous, dynamic feedback)
        {
            return Value;
        }
    }
}
