namespace FormulaFlow.Server.Intermediate.Dto
{
    public class OrderedLinkDto
    {
        public Guid Id { get; set; }
        public Guid From { get; set; }
        public int Order { get; set; }
    }
}
