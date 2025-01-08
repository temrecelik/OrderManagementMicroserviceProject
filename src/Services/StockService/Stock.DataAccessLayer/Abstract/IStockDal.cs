using Shared.Core.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Stock.DataAccessLayer.Abstract
{
    public interface IStockDal :IMongoRepository<EntityLayer.Concrete.Stock>
    {

    }
}
