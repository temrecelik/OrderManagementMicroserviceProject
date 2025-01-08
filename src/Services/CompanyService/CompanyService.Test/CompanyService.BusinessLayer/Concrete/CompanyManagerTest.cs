using AutoMapper;
using CompanyService.BusinessLayer.Concrete;
using CompanyService.DataAccessLayer.Abstract;
using CompanyService.EntityLayer.Concrete;
using CompanyService.EntityLayer.Dtos;
using CompanyService.Test.FakeDataGenerator;
using FakeItEasy;
using Shared.Core.CrossCuttingConcerns.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyService.Test.CompanyService.BusinessLayer.Concrete
{
    [TestFixture]
    public class CompanyManagerTest
    {
        private ICompanyDal _companyDal;
        private IMapper _mapper;
        private ICacheManager _cacheManager;
        private CompanyManager _companyManager;

        [SetUp]
        public void SetUp()
        {
            _companyDal = A.Fake<ICompanyDal>();
            _mapper = A.Fake<IMapper>();
            _cacheManager = A.Fake<ICacheManager>();
            _companyManager = new CompanyManager(_companyDal, _mapper, _cacheManager);

        }

        [Test]
        public void GetAll_GetCompanyDtoValues_returnGetCompanyDtoListWhenDataIsInCache()
        {
            var cacheKey = "CompanyManager.GetAll";
            var GetCompaniesDto = FakeDataGenerator.FakeDataGenerator.FakeGetCompanyDtoList();
            var companies = new List<Company>()
            {
                new Company {Id ="bedad86e-9c54-4749-9c13-a7d428a04e7d",OpeningTime = TimeSpan.Parse("10:00:00"),ClosingTime = TimeSpan.Parse("20:00:00"), Description = "Description1",Name = "Company1"},
                new Company {Id ="f7d2c916-0a6d-4649-9fa6-3750bc929154",OpeningTime =TimeSpan.Parse("08:00:00"),ClosingTime= TimeSpan.Parse("23:00:00"), Description="Description2",Name ="Company2"},
                new Company {Id= "41bb21c3-721c-4b7d-a50f-4fc7f4a9e5e3",OpeningTime =TimeSpan.Parse("12:00:00"),ClosingTime= TimeSpan.Parse("20:00:00"), Description="Description3",Name ="Company3"},
            };

            A.CallTo(() => _cacheManager.IsAdd(cacheKey)).Returns(true);
            A.CallTo(() => _cacheManager.Get<List<GetCompanyDto>>(cacheKey)).Returns(GetCompaniesDto);

            var result = _companyManager.GetAll();

            //Assert.AreEqual(GetCompaniesDto, result);
            Assert.That(result, Is.EqualTo(GetCompaniesDto));

            A.CallTo(() => _cacheManager.IsAdd(cacheKey)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _cacheManager.Get<List<GetCompanyDto>>(cacheKey)).MustHaveHappenedOnceExactly();

        }


        [Test]
        public void GetAll_GetCompanyDtoValues_returnGetCompanyDtoListWhenDataIsNotInCache()
        {

            var cacheKey = "CompanyManager.GetAll";
            var GetCompaniesDto = FakeDataGenerator.FakeDataGenerator.FakeGetCompanyDtoList();
            var companies = new List<Company>()
            {
                new Company {Id ="bedad86e-9c54-4749-9c13-a7d428a04e7d",OpeningTime = TimeSpan.Parse("10:00:00"),ClosingTime = TimeSpan.Parse("20:00:00"), Description = "Description1",Name = "Company1"},
                new Company {Id ="f7d2c916-0a6d-4649-9fa6-3750bc929154",OpeningTime =TimeSpan.Parse("08:00:00"),ClosingTime= TimeSpan.Parse("23:00:00"), Description="Description2",Name ="Company2"},
                new Company {Id= "41bb21c3-721c-4b7d-a50f-4fc7f4a9e5e3",OpeningTime =TimeSpan.Parse("12:00:00"),ClosingTime= TimeSpan.Parse("20:00:00"), Description="Description3",Name ="Company3"},
            }.AsQueryable();




            A.CallTo(() => _cacheManager.IsAdd(cacheKey)).Returns(false);
            A.CallTo(() => _companyDal.GetAll(null)).Returns(companies);
            A.CallTo(() => _mapper.Map<List<GetCompanyDto>>(companies)).Returns(GetCompaniesDto);
            A.CallTo(() => _cacheManager.Add(cacheKey, GetCompaniesDto, 60));

            var result = _companyManager.GetAll();

            //Assert.AreEqual(GetCompaniesDto, result);
            Assert.That(result, Is.EqualTo(GetCompaniesDto));

            A.CallTo(() => _cacheManager.IsAdd(cacheKey)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _companyDal.GetAll(null)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<List<GetCompanyDto>>(companies)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _cacheManager.Add(cacheKey, GetCompaniesDto, 60)).MustHaveHappenedOnceExactly();

        }


        [Test]

        public async Task GetById_GetCompanyDtoValues_ReturnCompanyDtoByIDWhenDataIsNotCache()
        {
            string id = "d6a682c3-9a3b-4c44-bd95-0e5f846ab728";

            string Cachekey = $"CompanyManager.GetByIdAsync.{id}";
            var company = new Company()
            {
                Id = id,
                Name = "Company1",
                OpeningTime = TimeSpan.Parse("10:00:00"),
                ClosingTime = TimeSpan.Parse("20:00:00"),
                Description = "Description1"

            };

            var ActualCompanyDto = FakeDataGenerator.FakeDataGenerator.FakeGetCompanyDto();

            A.CallTo(() => _cacheManager.IsAdd(Cachekey)).Returns(false);
            A.CallTo(() => _companyDal.GetByIdAsync(id)).Returns(Task.FromResult(company));
            A.CallTo(() => _mapper.Map<GetCompanyDto>(company)).Returns(ActualCompanyDto);

            var result = await _companyManager.GetByIdAsync(id);

            //Assert.AreEqual(ActualCompanyDto, result);
            Assert.That(result, Is.EqualTo(ActualCompanyDto));


            A.CallTo(() => _cacheManager.IsAdd(Cachekey)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _companyDal.GetByIdAsync(id)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _mapper.Map<GetCompanyDto>(company)).MustHaveHappenedOnceExactly();

        }

        [Test]
        public async Task GetById_GetCompanyDtoValues_ReturnCompanyDtoByIDWhenDataIsCache()
        {
            string id = "d6a682c3-9a3b-4c44-bd95-0e5f846ab728";

            string Cachekey = $"CompanyManager.GetByIdAsync.{id}";
            var company = new Company()
            {
                Id = id,
                Name = "Company1",
                OpeningTime = TimeSpan.Parse("10:00:00"),
                ClosingTime = TimeSpan.Parse("20:00:00"),
                Description = "Description1"

            };

            var ActualCompanyDto = FakeDataGenerator.FakeDataGenerator.FakeGetCompanyDto();

            A.CallTo(() => _cacheManager.IsAdd(Cachekey)).Returns(true);
            A.CallTo(() => _cacheManager.Get<GetCompanyDto>(Cachekey)).Returns(ActualCompanyDto);

            var result = await _companyManager.GetByIdAsync(id);

            //Assert.AreEqual(ActualCompanyDto, result);
            Assert.That(result, Is.EqualTo(ActualCompanyDto));

            A.CallTo(() => _cacheManager.IsAdd(Cachekey)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _cacheManager.Get<GetCompanyDto>(Cachekey)).MustHaveHappenedOnceExactly();

        }

        [Test]
        public async Task AddAsync_CompanyValues_AddCompanyToDatabase()
        {
            var createCompanyDto = FakeDataGenerator.FakeDataGenerator.FakeCreateCompanyDto();
            var createdCompany = new Company()
            {
                Name = createCompanyDto.Name,
            };

            A.CallTo(() => _mapper.Map<Company>(createCompanyDto)).Returns(createdCompany);
            A.CallTo(() => _companyDal.AddAsync(createdCompany)).Returns(Task.FromResult(true));
            A.CallTo(() => _companyDal.SaveAsync()).Returns(Task.FromResult(1));


            var result = await _companyManager.AddAsync(createCompanyDto);

            Assert.IsTrue(result);


            A.CallTo(() => _mapper.Map<Company>(createCompanyDto)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _companyDal.AddAsync(createdCompany)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _companyDal.SaveAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _cacheManager.RemoveByPattern("Company Manager.")).MustHaveHappenedOnceExactly();

        }

        [Test]
        public async Task UpdateAsync_CompanyValues_UpdateCompanyInDatabase()
        {
            var updateCompanyDto = FakeDataGenerator.FakeDataGenerator.FakeUpdateCompanyDto();
            var existcompany = FakeDataGenerator.FakeDataGenerator.FakeCompany();

            var updatedCompany = new Company
            {
                Id = updateCompanyDto.Id,
                Name = existcompany.Name,
                Description = existcompany.Description,
                OpeningTime = updateCompanyDto.OpeningTime,
                ClosingTime = updateCompanyDto.ClosingTime
            };

            A.CallTo(() => _companyDal.GetByIdAsync(updateCompanyDto.Id)).Returns(existcompany);
            A.CallTo(() => _mapper.Map(updateCompanyDto, existcompany)).Returns(updatedCompany);
            A.CallTo(() => _companyDal.UpdateAsync(updatedCompany, updateCompanyDto.Id)).Returns(Task.FromResult(true));

            var result = await _companyManager.UpdateAsync(updateCompanyDto);

            Assert.IsTrue(result);

            A.CallTo(() => _companyDal.GetByIdAsync(updateCompanyDto.Id)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _companyDal.UpdateAsync(updatedCompany, updatedCompany.Id)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _companyDal.SaveAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _cacheManager.RemoveByPattern("CompanyManager.*")).MustHaveHappenedOnceExactly();

        }

        [Test]
        public async Task RemoveAsync_CompanyValues_DeleteCompanyFromDatabase()
        {
            var existCompany = FakeDataGenerator.FakeDataGenerator.FakeCompany();
            var deletedCompanyId = existCompany.Id;


            A.CallTo(() => _companyDal.RemoveAsync(deletedCompanyId)).Returns(true);
            A.CallTo(() => _companyDal.SaveAsync()).Returns(1);

            var result = await _companyManager.RemoveAsync(deletedCompanyId);

            Assert.IsTrue(result);

            A.CallTo(() => _companyDal.RemoveAsync(deletedCompanyId)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _companyDal.SaveAsync()).MustHaveHappenedOnceExactly();

        }

    }
}
