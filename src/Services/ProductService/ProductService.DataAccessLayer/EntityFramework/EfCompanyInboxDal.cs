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
    public class EfCompanyInboxDal : MongoRepositoryBase<CompanyInbox, ProductServiceDbContext>, ICompanyInboxDal
    {
        private readonly IMongoCollection<CompanyInbox> _companyInboxCollection;

        public EfCompanyInboxDal(ProductServiceDbContext context) : base(context, "CompanyInBoxes")
        {
            _companyInboxCollection = context.GetCollection<CompanyInbox>("CompanyInBoxes");
        }
    }
}
