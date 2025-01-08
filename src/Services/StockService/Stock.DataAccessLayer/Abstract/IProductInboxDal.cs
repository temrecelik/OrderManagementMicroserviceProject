using Shared.Core.DataAccess.Abstract;
using Stock.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.DataAccessLayer.Abstract
{
    public interface IProductInboxDal : IMongoRepository<ProductInbox>
    {
    }
}
