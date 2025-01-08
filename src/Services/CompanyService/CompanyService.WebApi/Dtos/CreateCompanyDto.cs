using Shared.Core.Entities;

namespace CompanyService.EntityLayer.Dtos;

public class CreateCompanyDto : IDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
}