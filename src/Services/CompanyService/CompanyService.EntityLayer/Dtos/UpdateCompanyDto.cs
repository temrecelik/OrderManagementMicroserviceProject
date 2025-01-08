using Shared.Core.Entities;

namespace CompanyService.EntityLayer.Dtos;

public class UpdateCompanyDto : IDto
{
    public string Id { get; set; }
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
}