using Shared.Core.Entities;

namespace CompanyService.EntityLayer.Dtos;

public class RemoveCompanyDto : IDto
{
    public string Id { get; set; }
}