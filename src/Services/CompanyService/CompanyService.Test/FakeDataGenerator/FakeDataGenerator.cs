using CompanyService.EntityLayer.Concrete;
using CompanyService.EntityLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyService.Test.FakeDataGenerator
{
    public  class FakeDataGenerator
    {
        public static List<GetCompanyDto> FakeGetCompanyDtoList()
        {

            return new List<GetCompanyDto>
            {
                new GetCompanyDto {OpeningTime = TimeSpan.Parse("10:00:00"),ClosingTime = TimeSpan.Parse("20:00:00"), Description = "Description1",Name = "Company1"},
                new GetCompanyDto {OpeningTime =TimeSpan.Parse("08:00:00"),ClosingTime= TimeSpan.Parse("23:00:00"), Description="Description2",Name ="Company2"},
                new GetCompanyDto {OpeningTime =TimeSpan.Parse("12:00:00"),ClosingTime= TimeSpan.Parse("20:00:00"), Description="Description3",Name ="Company3"},
            };

        }

        public static GetCompanyDto FakeGetCompanyDto()
        {
            return new GetCompanyDto { OpeningTime = TimeSpan.Parse("10:00:00"), ClosingTime = TimeSpan.Parse("20:00:00"), Description = "Description1", Name = "Company1" };
        }


        public static CreateCompanyDto FakeCreateCompanyDto()
        {
            return new CreateCompanyDto { Name = "Company1" };
        }


        public static UpdateCompanyDto FakeUpdateCompanyDto()
        {
            return new UpdateCompanyDto { Id = "d6a682c3-9a3b-4c44-bd95-0e5f846ab728", OpeningTime = TimeSpan.Parse("10:00:00"), ClosingTime = TimeSpan.Parse("20:00:00") };
        }

        public static RemoveCompanyDto FakeRemoveDto()
        {
            return new RemoveCompanyDto { Id = "d6a682c3-9a3b-4c44-bd95-0e5f846ab728" };
        }

        public static Company FakeCompany()
        {
            return new Company { Id = "d6a682c3-9a3b-4c44-bd95-0e5f846ab728", Name = "Company1", OpeningTime = TimeSpan.Parse("10:00:00"), ClosingTime = TimeSpan.Parse("20:00:00"), Description = "Description1" };
        }
    }
}
