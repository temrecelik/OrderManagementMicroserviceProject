using Order.EntityLayer.Concrete;
using Shared.Core.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.DataAccessLayer.Abstract
{
    public  interface IOrderItemDal : IMsSqlRepository<OrderItem>
    {
    }
}
