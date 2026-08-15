using FormulaFlow.Server.Intermediate.Dto;
using FormulaFlow.Server.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;

namespace FormulaFlow.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BackTestController : ControllerBase
    {
        private IBackTestService _backTestService;

        public BackTestController(IBackTestService backTestService)
        {
            _backTestService = backTestService;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<IEnumerable<BackTestResultDto>>> Post(Guid id, [FromQuery, BindRequired] DateTime start, [FromQuery, BindRequired] DateTime end)
        {
            var dtos = await _backTestService.Get(id, start, end);

            var csvBuilder = new StringBuilder();

            foreach (var line in dtos)
            {
                csvBuilder.AppendLine($"{line.Date.ToUniversalTime().ToShortDateString()}, {line.Value}");
            }

            var bytesOutput = Encoding.UTF8.GetBytes(csvBuilder.ToString());

            return File(bytesOutput, "application/csv", $"{id}-{start}-{end}.csv");
        }
    }
}
