using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.DataAccessLayer
{
    public class Configuration
    {
        public static string? ConnectionString(string connectionName)
        {
            ConfigurationManager configurationManager = new();
            configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Order.WebApi"));
            configurationManager.AddJsonFile("appsettings.json");

            return configurationManager.GetConnectionString(connectionName);

        }
    }
}
