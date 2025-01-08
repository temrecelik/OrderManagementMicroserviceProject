using Microsoft.Extensions.Configuration;


namespace Payment.DataAccessLayer
{
    public class Configuration
    {
        public static string? ConnectionString(string connectionName)
        {
            ConfigurationManager configurationManager = new();
            configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Payment.WebApi"));
            configurationManager.AddJsonFile("appsettings.json");

            return configurationManager.GetConnectionString(connectionName);

        }
    }
}
