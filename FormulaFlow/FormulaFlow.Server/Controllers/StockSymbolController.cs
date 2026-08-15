using FormulaFlow.Data.Models;
using FormulaFlow.Server.Dto;
using FormulaFlow.Server.Service.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace FormulaFlow.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StockSymbolController : ControllerBase
    {
        private readonly IServiceBase<StockSymbol, StockSymbolDto> _service;
        public StockSymbolController(
            IServiceBase<StockSymbol, StockSymbolDto> service
            )
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StockSymbolDto>> GetById(Guid id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            return Ok(dto);
        }

        [HttpGet]
        public async Task<ActionResult<PagedData<StockSymbolDto>>> GetPaged([FromQuery, BindRequired] int page, [FromQuery, BindRequired] int pageSize)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var paged = await _service.GetPagedAsync(page, pageSize);
            return Ok(paged);
        }

        [HttpPost]
        public async Task<ActionResult<StockSymbolDto>> Post([FromBody] StockSymbolDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var created = await _service.AddAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<StockSymbolDto>> Update(Guid id, [FromBody] StockSymbolDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var updated = await _service.Update(id, dto, userId);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _service.Delete(id);
            return NoContent();
        }

    }
}
