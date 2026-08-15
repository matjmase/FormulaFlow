using FormulaFlow.Server.Intermediate.Dto;
using FormulaFlow.Server.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormulaFlow.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CardCatalogController
    {
        private readonly ICardCatalogService _catalogService;

        public CardCatalogController(ICardCatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        public async Task<IEnumerable<StockCardDto>> Get()
        {
            return await _catalogService.Get();
        }
    }
}
