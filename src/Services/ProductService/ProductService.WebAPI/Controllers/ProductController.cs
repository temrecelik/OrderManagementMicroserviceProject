using Microsoft.AspNetCore.Mvc;
using ProductService.BusinessLayer.Abstract;
using ProductService.DtoLayer.ProductDtos;

namespace ProductService.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var values = await _productService.GetAllAsync();
            return Ok(values);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProductWithCategory()
        {
            var values = await _productService.GetProductWithCategory();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var value = await _productService.GetByIdAsync(id);
            return Ok(value);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductWithCategoryById(string id)
        {
            var value = await _productService.GetProductWithCategoryById(id);
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto product)
        {
            await _productService.AddAsync(product);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto product)
        {
            await _productService.UpdateAsync(product);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _productService.DeleteAsync(id);
            return Ok();
        }
    }
}
