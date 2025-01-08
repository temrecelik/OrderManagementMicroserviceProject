using CompanyService.EntityLayer.Concrete;
using Shared.Core.DataAccess.Abstract;

namespace CompanyService.DataAccessLayer.Abstract;

public interface ICompanyDal : IMsSqlRepository<Company>
{

}