namespace FormulaFlow.Server.Intermediate.Formula.Model.Computable
{
    public class FeedbackComputable : IComputable
    {
        public dynamic Compute(IndexShiftArrayHolder previous, dynamic feedback)
        {
            return feedback;
        }
    }
}
