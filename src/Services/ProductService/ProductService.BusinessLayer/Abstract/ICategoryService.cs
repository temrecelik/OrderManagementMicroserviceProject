using ProductService.DtoLayer.CategoryDtos;
using ProductService.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.BusinessLayer.Abstract
{
    public interface ICategoryService:IGenericService<ResultCategoryDto,UpdateCategoryDto,CreateCategoryDto>
    {
    }
}
