using ProductService.BusinessLayer.Abstract;
using ProductService.BusinessLayer.Mapping;
using ProductService.DataAccessLayer.Abstract;
using ProductService.DtoLayer.CategoryDtos;
using ProductService.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Serializers;

namespace ProductService.BusinessLayer.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public async Task AddAsync(CreateCategoryDto entity)
        {
            await _categoryDal.AddAsync(ObjectMapper.mapper.Map<Category>(entity));
        }

        public async Task DeleteAsync(string id)
        {
            await _categoryDal.RemoveAsync(id);
        }

        public async Task<List<ResultCategoryDto>> GetAllAsync()
        {
            var values = await _categoryDal.GetAllAsync();
            return ObjectMapper.mapper.Map<List<ResultCategoryDto>>(values);
        }

        public async Task<ResultCategoryDto> GetByIdAsync(string id)
        {
            var value = await _categoryDal.GetByIdAsync(id);
            return ObjectMapper.mapper.Map<ResultCategoryDto>(value);
        }

        public async Task UpdateAsync(UpdateCategoryDto entity)
        {
            await _categoryDal.UpdateAsync(ObjectMapper.mapper.Map<Category>(entity), entity.CategoryId);
        }
    }
}
