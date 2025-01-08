using AutoMapper;
using CompanyService.EntityLayer.Concrete;
using CompanyService.EntityLayer.Dtos;

namespace CompanyService.BusinessLayer.Profiles;

public class CompanyMappingProfile : Profile
{
    public CompanyMappingProfile()
    {
        CreateMap<Company, GetCompanyDto>().ReverseMap();
        CreateMap<Company, CreateCompanyDto>().ReverseMap();
        CreateMap<Company, UpdateCompanyDto>().ReverseMap();
        CreateMap<Company, RemoveCompanyDto>().ReverseMap();
    }
}