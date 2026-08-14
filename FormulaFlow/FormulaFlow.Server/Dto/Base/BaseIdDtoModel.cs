namespace FormulaFlow.Server.Dto.Base
{
    public class BaseIdDtoModel
    {
        public Guid? Id { get; set; }
        public string? CreatedByUserId { get; set; }
        public string? UpdatedByUserId { get; set; }
    }
}
