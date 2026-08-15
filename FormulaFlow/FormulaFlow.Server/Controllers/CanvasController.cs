using FormulaFlow.Data.Models;
using FormulaFlow.Server.Dto;
using FormulaFlow.Server.Service.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FormulaFlow.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CanvasController : ControllerBase
    {
        private readonly IServiceBase<NetworkCanvas, StockCanvasSimpleDto> _service;

        public CanvasController(IServiceBase<NetworkCanvas, StockCanvasSimpleDto> service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StockCanvasSimpleDto>> GetById(Guid id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            return Ok(dto);
        }

        [HttpGet]
        public async Task<ActionResult<PagedData<StockCanvasSimpleDto>>> GetPaged([FromQuery, BindRequired] int page, [FromQuery, BindRequired] int pageSize)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var paged = await _service.GetPagedAsync(page, pageSize);
            return Ok(paged);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] StockCanvasSimpleDto dto)
        {
            var id = dto.Id;

            if (id == null)
            {
                return NotFound();
            }

            await _service.Delete((Guid)id);
            return NoContent();
        }
    }
}
