using MongoDB.Driver;
using ProductService.DataAccessLayer.Abstract;
using ProductService.DataAccessLayer.Context;
using ProductService.EntityLayer.Concrete;
using Shared.Core.DataAccess.Repositories;

namespace ProductService.DataAccessLayer.EntityFramework
{
    public class EfProductDal : MongoRepositoryBase<Product, ProductServiceDbContext>, IProductDal
    {
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMongoCollection<Category> _categoryCollection;

        public EfProductDal(ProductServiceDbContext context) : base(context, "Products")
        {
            _productCollection = context.GetCollection<Product>("Products");
            _categoryCollection = context.GetCollection<Category>("Categories");
        }

        public async Task<List<Product>> GetProductWithCategory()
        {
            var products = await _productCollection.Find(_ => true).ToListAsync();

            foreach (var product in products)
            {
                if (!string.IsNullOrEmpty(product.CategoryId))
                {
                    product.Category = await _categoryCollection
                        .Find(c => c.CategoryId == product.CategoryId)
                        .FirstOrDefaultAsync();
                }
            }

            return products;
        }

        public async Task<Product> GetProductWithCategoryById(string id)
        {
            var product = await _productCollection
                .Find(p => p.ProductId == id)
                .FirstOrDefaultAsync();

            if (product != null && !string.IsNullOrEmpty(product.CategoryId))
            {
                product.Category = await _categoryCollection
                    .Find(c => c.CategoryId == product.CategoryId)
                    .FirstOrDefaultAsync();
            }

            return product;
        }
    }
}
