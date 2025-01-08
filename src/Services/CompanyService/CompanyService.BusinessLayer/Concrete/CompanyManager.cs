using AutoMapper;
using CompanyService.BusinessLayer.Abstract;
using CompanyService.DataAccessLayer.Abstract;
using CompanyService.EntityLayer.Concrete;
using CompanyService.EntityLayer.Dtos;
using Shared.Core.CrossCuttingConcerns.Caching;

namespace CompanyService.BusinessLayer.Concrete;

public class CompanyManager : ICompanyService
{
    private readonly ICompanyDal _companyDal;
    private readonly IMapper _mapper;
    private readonly ICacheManager _cacheManager;

    public CompanyManager(ICompanyDal companyDal, IMapper mapper, ICacheManager cacheManager)
    {
        _companyDal = companyDal;
        _mapper = mapper;
        _cacheManager = cacheManager;
    }

    public List<GetCompanyDto> GetAll()
    {
        const string cacheKey = "CompanyManager.GetAll";

        if (_cacheManager.IsAdd(cacheKey))
        {
            return _cacheManager.Get<List<GetCompanyDto>>(cacheKey);
        }

        var companies = _companyDal.GetAll(null);
        var getCompaniesDto = _mapper.Map<List<GetCompanyDto>>(companies);

        _cacheManager.Add(cacheKey, getCompaniesDto, duration: 60);
        return getCompaniesDto;
    }


    public async Task<GetCompanyDto> GetByIdAsync(string id)
    {
        string Cachekey = $"CompanyManager.GetByIdAsync.{id}";

        if (_cacheManager.IsAdd(Cachekey))
            return _cacheManager.Get<GetCompanyDto>(Cachekey);

        Company company = await _companyDal.GetByIdAsync(id);
        GetCompanyDto getCompanyDto = _mapper.Map<GetCompanyDto>(company);

        _cacheManager.Add(Cachekey, getCompanyDto, duration: 60);
        return getCompanyDto;
    }

    public async Task<bool> AddAsync(CreateCompanyDto createCompanyDto)
    {
        var entity = _mapper.Map<Company>(createCompanyDto);

        await _companyDal.AddAsync(entity);
        await _companyDal.SaveAsync();

        _cacheManager.RemoveByPattern("Company Manager.");
        return true;
    }

    public async Task<bool> UpdateAsync(UpdateCompanyDto updateCompanyDto)
    {
        var company = await _companyDal.GetByIdAsync(updateCompanyDto.Id);
        company = _mapper.Map(updateCompanyDto, company);

        await _companyDal.UpdateAsync(company,company.Id);//id kısmı yoktu
        await _companyDal.SaveAsync();

        _cacheManager.RemoveByPattern("CompanyManager.*");

        return true;
    }

    public async Task<bool> RemoveAsync(string id)
    {
        await _companyDal.RemoveAsync(id);
        await _companyDal.SaveAsync();

        _cacheManager.RemoveByPattern("CompanyManager.*");

        return true;
    }
}