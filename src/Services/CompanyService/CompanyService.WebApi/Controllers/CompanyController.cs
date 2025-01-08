using CompanyService.BusinessLayer.Abstract;
using CompanyService.EntityLayer.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CompanyService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ICompanyService companyService, ILogger<CompanyController> logger)
        {
            _companyService = companyService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("CompanyLog");
            var result = _companyService.GetAll();
            return Ok(result);
        }

        [HttpGet(template: "id")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _companyService.GetByIdAsync(id);
            return Ok(result);
        }

        //[HttpPost]
        //public async Task<IActionResult> Post(CreateCompanyDto  createCompanyDto)
        //{
        //    var result = await _companyService.AddAsync(createCompanyDto);
        //    if (result)
        //    {
        //        return Ok("Eklendi");
        //    }
        //    _logger.LogError(message: "Post Endpoint Hata", args: createCompanyDto);
        //    return BadRequest("Hata");
        //}

        //[HttpPut]
        //public async Task<IActionResult> Put(UpdateCompanyDto updateCompanyDto)
        //{
        //    var result = await _companyService.UpdateAsync(updateCompanyDto);
        //    if (result)
        //    {
        //        return Ok("Güncellendi");
        //    }
        //    _logger.LogError(message: "Put Endpoint Hata", args: updateCompanyDto);
        //    return BadRequest("Hata");
        //}

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _companyService.RemoveAsync(id);
            if (result)
            {
                return Ok("Silindi");
            }
            _logger.LogError(message: "Delete Endpoint Hata", args: id);
            return BadRequest("Hata");
        }
    }
}
