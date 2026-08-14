namespace FormulaFlow.Server.Dto.Base
{
    public class NoSqlBaseIdDtoModel
    {
        public Guid? Id { get; set; }
        public string? CreatedByUserId { get; set; }
        public string? UpdatedByUserId { get; set; }
    }
}
