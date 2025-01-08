using ProductService.EntityLayer.Concrete;
using Shared.Core.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.DataAccessLayer.Abstract
{ 
    public interface IProductOutboxDal :IMongoRepository<ProductOutbox>
    {
    }
}
