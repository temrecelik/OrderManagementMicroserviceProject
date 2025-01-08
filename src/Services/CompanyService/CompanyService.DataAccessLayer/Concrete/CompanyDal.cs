using CompanyService.DataAccessLayer.Abstract;
using CompanyService.DataAccessLayer.Context;
using CompanyService.EntityLayer.Concrete;
using Shared.Core.DataAccess.Repositories;

namespace CompanyService.DataAccessLayer.Concrete;

public class CompanyDal : MsSqlRepositoryBase<Company, CompanyDbContext>, ICompanyDal
{
    public CompanyDal(CompanyDbContext context) : base(context)
    { }
}