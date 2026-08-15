using FormulaFlow.Server.Intermediate.Dto;

namespace FormulaFlow.Server.Intermediate.Formula.Model
{
    public struct IndexShiftArrayHolder
    {
        private BackTestResultDto[][] _array;
        private Dictionary<int, int> _dimShift;
        private int _indexShift;

        public IndexShiftArrayHolder(BackTestResultDto[][] array, Dictionary<int, int> dimShift, int indexShift)
        {
            _array = array;
            _dimShift = dimShift;
            _indexShift = indexShift;
        }

        public BackTestResultDto GetValue(int dim1, int dim2)
        {
            var maxBuffer = 0;

            if (_dimShift.ContainsKey(dim1))
            {
                maxBuffer = _dimShift[dim1];
            }

            return _array[dim1][maxBuffer + _indexShift - dim2];
        }
    }
}
