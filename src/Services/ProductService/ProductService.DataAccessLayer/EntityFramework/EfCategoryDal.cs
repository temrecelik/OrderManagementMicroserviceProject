using ProductService.DataAccessLayer.Abstract;
using ProductService.DataAccessLayer.Context;
using ProductService.EntityLayer.Concrete;
using Shared.Core.DataAccess.Repositories;

namespace ProductService.DataAccessLayer.EntityFramework
{
    public class EfCategoryDal : MongoRepositoryBase<Category, ProductServiceDbContext>, ICategoryDal
    {
        public EfCategoryDal(ProductServiceDbContext context) : base(context, "Categories")
        {
        }
    }
}
