using FormulaFlow.Server.Intermediate.Dto;
using FormulaFlow.Server.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FormulaFlow.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CanvasComplexController : ControllerBase
    {
        private readonly IStockCanvasCompositeService _service;

        public CanvasComplexController(IStockCanvasCompositeService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<StockCanvasDto>> Post([FromBody] StockCanvasDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var created = await _service.Add(dto, userId);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StockCanvasDto>> Get(Guid id)
        {
            var dto = await _service.Get(id);
            if (dto == null)
                return NotFound();

            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<StockCanvasDto>> Update(Guid id, [FromBody] StockCanvasDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            dto.Id = id;
            var updated = await _service.Update(dto, userId);
            return Ok(updated);
        }
    }
}
