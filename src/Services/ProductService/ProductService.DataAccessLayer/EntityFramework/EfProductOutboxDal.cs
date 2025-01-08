using MongoDB.Driver;
using ProductService.DataAccessLayer.Abstract;
using ProductService.DataAccessLayer.Context;
using ProductService.EntityLayer.Concrete;
using Shared.Core.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.DataAccessLayer.EntityFramework
{
    public class EfProductOutboxDal : MongoRepositoryBase<ProductOutbox, ProductServiceDbContext>, IProductOutboxDal
    {
        private readonly IMongoCollection<ProductOutbox> _productOutboxCollection;

        public EfProductOutboxDal(ProductServiceDbContext context) : base(context, "ProductOutboxes")
        {
           _productOutboxCollection = context.GetCollection<ProductOutbox>("ProductOutboxes");
        }
    }
}
