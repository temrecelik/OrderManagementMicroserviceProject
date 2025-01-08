using Shared.Core.Entities;

namespace CompanyService.EntityLayer.Dtos;

public class GetCompanyDto : IDto
{
    public string Name { get; set; }
    public string Description { get; set; }

    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
}