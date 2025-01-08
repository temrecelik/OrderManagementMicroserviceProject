using Amazon.Runtime.Internal.Util;
using ProductService.BusinessLayer.Abstract;
using ProductService.BusinessLayer.Mapping;
using ProductService.DataAccessLayer.Abstract;
using ProductService.DtoLayer.ProductDtos;
using ProductService.EntityLayer.Concrete;
using Shared.Core.CrossCuttingConcerns.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.BusinessLayer.Concrete
{
    public class ProductManager : IProductService
    {
        private readonly IProductDal _productDal;
        private readonly ICacheManager _cacheManager;

        public ProductManager(IProductDal productDal, ICacheManager cacheManager)
        {
            _productDal = productDal;
            _cacheManager = cacheManager;
        }

        public async Task AddAsync(CreateProductDto entity)
        {
            await _productDal.AddAsync(ObjectMapper.mapper.Map<Product>(entity));
            _cacheManager.RemoveByPattern("Product");
        }

        public async Task DeleteAsync(string id)
        {
            await _productDal.RemoveAsync(id);
            _cacheManager.RemoveByPattern("Product");
        }

        public async Task<List<ResultProductDto>> GetAllAsync()
        {
            string cacheKey = "Product.GetAll";
            if (_cacheManager.IsAdd(cacheKey))
            {
                   return _cacheManager.Get<List<ResultProductDto>>(cacheKey); 
            }


             var values = await _productDal.GetAllAsync();
           
            
            _cacheManager.Add(cacheKey, ObjectMapper.mapper.Map<List<ResultProductDto>>(values), 60);
            
            return ObjectMapper.mapper.Map<List<ResultProductDto>>(values);

        }

        public async Task<ResultProductDto> GetByIdAsync(string id)
        {
            string  cacheKey = $"Product.GetById.{id}";

            if (_cacheManager.IsAdd(cacheKey))
            {
                return _cacheManager.Get<ResultProductDto>(cacheKey);
            }

            var value = await _productDal.GetByIdAsync(id);

            
            var result = ObjectMapper.mapper.Map<ResultProductDto>(value);

            _cacheManager.Add(cacheKey, result, 60);
            return result;
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductWithCategory()
        {
            string cacheKey = "Product.GetProductWithCategory";
            if (_cacheManager.IsAdd(cacheKey))
            {
                return _cacheManager.Get<List<ResultProductWithCategoryDto>>(cacheKey);
            }


            var values = await _productDal.GetProductWithCategory();

            var results = values.Select(x => new ResultProductWithCategoryDto
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                ProductDescription = x.ProductDescription,
                ProductPrice = x.ProductPrice,
                CategoryId = x.CategoryId,
                CategoryName = x.Category?.CategoryName
            }).ToList();

            _cacheManager.Add(cacheKey, results, 60);
            return results;
        }

        public async Task<ResultProductWithCategoryDto> GetProductWithCategoryById(string id)
        {
            string cacheKey = $"Product.GetProductWithCategoryById.{id}";
            if (_cacheManager.IsAdd(cacheKey))
            {
                return _cacheManager.Get<ResultProductWithCategoryDto>(cacheKey);
            }


            var value = await _productDal.GetProductWithCategoryById(id);

            var result = new ResultProductWithCategoryDto
            {
                ProductId = value.ProductId,
                ProductName = value.ProductName,
                ProductDescription = value.ProductDescription,
                ProductPrice = value.ProductPrice,
                CategoryId = value.CategoryId,
                CategoryName = value.Category?.CategoryName
            };

            _cacheManager.Add(cacheKey, result, 60);
            return result;
        }

        public async Task UpdateAsync(UpdateProductDto entity)
        {
            await _productDal.UpdateAsync(ObjectMapper.mapper.Map<Product>(entity), entity.ProductId);
            _cacheManager.RemoveByPattern("Product");
        }
    }
}
