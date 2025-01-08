using Shared.Core.Entities;

namespace CompanyService.EntityLayer.Concrete;

public class Company : IEntity
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }

    public Company(string id)
    {
        Id = id;
    }
    public Company()
    {
        Id = Guid.NewGuid().ToString();
    }
}