using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stock.BusinessLayer.Absract;
using Stock.EntityLayer.Dtos;

namespace Stock.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateCategory(StockDto category)
        {
            await _stockService.AddAsync(category);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(StockUpdateDto category)
        {
            await _stockService.UpdateAsync(category);
            return Ok();
        }
    }
}
