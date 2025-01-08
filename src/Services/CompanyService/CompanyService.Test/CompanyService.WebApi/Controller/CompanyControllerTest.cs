using CompanyService.BusinessLayer.Abstract;
using CompanyService.WebApi.Controllers;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyService.Test.CompanyService.WebApi.Controller
{
    [TestFixture]
    public class CompanyControllerTest
    {
        private CompanyController _companyController;
        private ICompanyService _companyService;
        private ILogger<CompanyController> _logger;

        [SetUp]

         public void Setup()
        {
            _companyService = A.Fake<ICompanyService>();
            _logger = A.Fake<ILogger<CompanyController>>();
            _companyController = new CompanyController(_companyService, _logger);
        }


        [Test]

        public void Get_CompanyDtoValues_returnOkwithCompanyDtoList()
        {
            var getCompanyDto = FakeDataGenerator.FakeDataGenerator.FakeGetCompanyDtoList();

          
            A.CallTo(() => _companyService.GetAll()).Returns(getCompanyDto);
            var result = _companyController.Get();

            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(getCompanyDto, okResult.Value);

          
            A.CallTo(() => _companyService.GetAll()).MustHaveHappenedOnceExactly();
        }
    }


}
