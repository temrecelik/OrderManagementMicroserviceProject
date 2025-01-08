using CompanyService.EntityLayer.Concrete;
using CompanyService.EntityLayer.Dtos;

namespace CompanyService.BusinessLayer.Abstract;

public interface ICompanyService
{
    List<GetCompanyDto> GetAll();
    Task<GetCompanyDto> GetByIdAsync(string id);
    Task<bool> AddAsync(CreateCompanyDto  createCompanyDto);
    Task<bool> UpdateAsync(UpdateCompanyDto updateCompanyDto);
    Task<bool> RemoveAsync(string id);
}