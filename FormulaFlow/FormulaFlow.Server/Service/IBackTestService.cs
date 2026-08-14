using FormulaFlow.Server.Intermediate.Dto;

namespace FormulaFlow.Server.Service
{
    public interface IBackTestService
    {
        public Task<IEnumerable<BackTestResultDto>> Get(Guid cardGuid, DateTime start, DateTime end);
    }
}
