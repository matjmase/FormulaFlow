namespace FormulaFlow.Server.Dto
{
    public class UploadFileModelDto
    {
        public bool SkipHeader { get; set; }
        public int DateColumnIndex { get; set; }
        public int ValueColumnIndex { get; set; }
        public UploadFileModelDtoCollisionBehavior CollisionBehavior { get; set; }
    }

    public enum UploadFileModelDtoCollisionBehavior
    {
        SkipExisting,
        OverwriteExisting,
        CreateNewEntry
    }
}
