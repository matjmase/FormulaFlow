namespace FormulaFlow.Server.Intermediate.Formula.Model.Computable
{
    public class UnaryOperationComputable : IComputable
    {
        public IComputable Right;

        public Func<dynamic, dynamic> Function;
        public dynamic Compute(IndexShiftArrayHolder previous, dynamic feedback)
        {
            return Function(Right.Compute(previous, feedback));
        }
    }
}
