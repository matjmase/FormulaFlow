namespace FormulaFlow.Server.Intermediate.Formula.Model.Computable
{
    public class BinaryOperationComputable : IComputable
    {
        public IComputable Left;
        public IComputable Right;

        public Func<dynamic, dynamic, dynamic> Function;

        public dynamic Compute(IndexShiftArrayHolder previous, dynamic feedback)
        {
            return Function(Left.Compute(previous, feedback), Right.Compute(previous, feedback));
        }
    }
}
