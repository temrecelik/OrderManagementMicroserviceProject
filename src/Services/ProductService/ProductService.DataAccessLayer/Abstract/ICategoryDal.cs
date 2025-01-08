using ProductService.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core.DataAccess.Abstract;

namespace ProductService.DataAccessLayer.Abstract
{
    public interface ICategoryDal : IMongoRepository<Category>
    {
    }
}
