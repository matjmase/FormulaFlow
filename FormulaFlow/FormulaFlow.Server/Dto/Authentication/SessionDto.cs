namespace FormulaFlow.Server.Dto.Authentication
{
    public class SessionDto
    {
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
