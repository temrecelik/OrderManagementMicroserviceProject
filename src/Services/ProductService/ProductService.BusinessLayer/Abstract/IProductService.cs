using ProductService.DtoLayer.ProductDtos;
using ProductService.EntityLayer.Concrete;

namespace ProductService.BusinessLayer.Abstract
{
    public interface IProductService:IGenericService<ResultProductDto, UpdateProductDto, CreateProductDto>
    {
        Task<List<ResultProductWithCategoryDto>> GetProductWithCategory();
        Task<ResultProductWithCategoryDto> GetProductWithCategoryById(string id);
    }
}
