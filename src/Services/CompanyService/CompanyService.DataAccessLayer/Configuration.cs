using Microsoft.Extensions.Configuration;

namespace CompanyService.DataAccessLayer;

public class Configuration
{
    public static string? ConnectionString(string connectionName)
    {
        ConfigurationManager configurationManager = new();
        configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../CompanyService.WebApi"));
        configurationManager.AddJsonFile("appsettings.json");

        return configurationManager.GetConnectionString(connectionName);

    }
}