using Order.DataAccessLayer.Abstract;
using Order.DataAccessLayer.Context;
using Order.EntityLayer.Concrete;
using Shared.Core.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.DataAccessLayer.Concrete
{
    public class EfOrderDal : MsSqlRepositoryBase<EntityLayer.Concrete.Order, OrderDbContext>, IOrderDal
    {
        public EfOrderDal(OrderDbContext context) : base(context)
        {
        }
    }
}
