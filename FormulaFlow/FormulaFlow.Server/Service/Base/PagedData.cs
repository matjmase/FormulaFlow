namespace FormulaFlow.Server.Service.Base
{
    public class PagedData<T>
    {
        // Collection of records for the current page
        public IEnumerable<T> Record { get; set; } = new List<T>();

        // Zero-based page index
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int RecordCount { get; set; }

        public int TotalPages => PageSize == 0 ? 0 : (int)System.Math.Ceiling(RecordCount / (double)PageSize);
    }
}
