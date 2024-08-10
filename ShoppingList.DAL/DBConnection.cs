using Microsoft.Extensions.Configuration;

namespace ShoppingList.DAL
{
    public static class DBConnection
    {
        public static string GetDBConnection(string connectionName)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return configBuilder.Build().GetConnectionString(connectionName);

        }
    }
}
