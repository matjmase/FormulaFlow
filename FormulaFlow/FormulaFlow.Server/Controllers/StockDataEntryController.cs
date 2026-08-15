using FormulaFlow.Data.NoSql.Models;
using FormulaFlow.Server.Dto;
using FormulaFlow.Server.NoSql.Service.Base;
using FormulaFlow.Server.Service.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Security.Claims;

namespace FormulaFlow.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StockDataEntryController : ControllerBase
    {
        private readonly INoSqlService<StockDataEntry, StockDataEntryDto> _service;

        public StockDataEntryController(
            INoSqlService<StockDataEntry, StockDataEntryDto> service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StockDataEntryDto>> GetById(Guid id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            return Ok(dto);
        }

        [HttpGet]
        public async Task<ActionResult<PagedData<StockDataEntryDto>>> GetPaged(
            [FromQuery, BindRequired] int page,
            [FromQuery, BindRequired] int pageSize,
            [FromQuery] Guid? stockSymbolId,
            [FromQuery] DateTimeOffset? startDate,
            [FromQuery] DateTimeOffset? endDate
            )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Expression<Func<StockDataEntry, bool>> filter = entity =>
                (stockSymbolId == null || entity.StockSymbolId == stockSymbolId) &&
                (startDate == null || entity.Date >= startDate) &&
                (endDate == null || entity.Date <= endDate);


            var paged = await _service.GetPagedAsync(page - 1, pageSize, filter);
            return Ok(paged);
        }

        [HttpPost("{stockSymbolId}")]
        public async Task<ActionResult> UploadFile([FromRoute] Guid stockSymbolId, IFormFile file, [FromForm] string strModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            if (file == null || file.Length == 0)
                return BadRequest("File is required.");

            var model = JsonConvert.DeserializeObject<UploadFileModelDto>(strModel);
            if (model == null)
                return BadRequest("Invalid model.");

            string content;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                content = await reader.ReadToEndAsync();
            }

            if (string.IsNullOrWhiteSpace(content))
                return BadRequest("File is empty.");

            var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (model.SkipHeader && lines.Count > 0)
                lines.RemoveAt(0);

            // Get all additions
            var toAdd = new Dictionary<DateTime, StockDataEntryDto>();

            foreach (var line in lines)
            {
                var values = line.Split(',');
                if (values.Length <= Math.Max(model.DateColumnIndex, model.ValueColumnIndex))
                    continue;

                if (!DateTime.TryParse(values[model.DateColumnIndex], out var date))
                    continue;

                var doubleString = values[model.ValueColumnIndex].Replace("\"", "").Replace("$", "").Trim();
                if (!double.TryParse(doubleString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var value))
                {
                    if (!double.TryParse(doubleString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out value))
                        continue;
                }

                var dto = new StockDataEntryDto
                {
                    Date = date,
                    Amount = value,
                    StockSymbolId = stockSymbolId,
                };

                toAdd.Add(date, dto);
            }

            var maxDate = toAdd.Keys.Max();
            var mindate = toAdd.Keys.Min();

            var existing = (await _service.GetAllAsync(e => e.StockSymbolId == stockSymbolId && e.Date <= maxDate && e.Date >= mindate)).ToDictionary(obj => obj.Date);

            var intersectingDates = toAdd.Keys.Intersect(existing.Keys).ToHashSet();

            switch (model.CollisionBehavior)
            {
                case UploadFileModelDtoCollisionBehavior.SkipExisting:
                    foreach (var intersectingDate in intersectingDates)
                    {
                        toAdd.Remove(intersectingDate);
                    }
                    break;
                case UploadFileModelDtoCollisionBehavior.OverwriteExisting:
                    foreach (var intersectingDate in intersectingDates)
                    {
                        await _service.Delete((Guid)existing[intersectingDate].Id);
                    }
                    break;
                case UploadFileModelDtoCollisionBehavior.CreateNewEntry:
                    if (toAdd.Keys.Intersect(existing.Keys).Any())
                    {
                        throw new Exception("Collision detected. There are existing entries with the same date. Please choose a different collision behavior.");
                    }
                    break;
                default: throw new NotImplementedException();
            }

            foreach (var addition in toAdd.Values)
            {
                await _service.AddAsync(addition, userId);
            }

            return Ok();
        }
    }
}
