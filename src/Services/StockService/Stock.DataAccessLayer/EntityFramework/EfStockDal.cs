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
    public class EfStockDal : MongoRepositoryBase<EntityLayer.Concrete.Stock, StockDbContext>, IStockDal
    {
        public EfStockDal(StockDbContext context) : base(context, "Stocks")
        {
        }
    }
}
