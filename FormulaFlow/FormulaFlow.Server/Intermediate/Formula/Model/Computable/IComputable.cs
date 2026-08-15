namespace FormulaFlow.Server.Intermediate.Formula.Model.Computable
{
    public interface IComputable
    {
        public dynamic Compute(IndexShiftArrayHolder previous, dynamic feedback);
    }
}
