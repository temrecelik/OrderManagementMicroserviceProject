using MongoDB.Driver;
using Shared.Core.DataAccess.Repositories;
using Stock.DataAccessLayer.Abstract;
using Stock.DataAccessLayer.Context;
using Stock.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.DataAccessLayer.EntityFramework
{
    public class EfProductInboxDal : MongoRepositoryBase<ProductInbox, StockDbContext>, IProductInboxDal
    {
        private readonly IMongoCollection<ProductInbox> _productInboxCollection;

        public EfProductInboxDal(StockDbContext context) : base(context, "ProductInboxes")
        {
            _productInboxCollection = context.GetCollection<ProductInbox>("ProductInboxes");
        }
    }
}
