using DataAccess.Dapper.Interfaces.Settings;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Dapper.Settings;

public class DapperSettings : IDapperSettings
{
    public DapperSettings(IConfiguration configuration)
    {
        ConnectionString = configuration.GetConnectionString("Database") ??
                           throw new ArgumentException("Set database connection string");
    }
    
    public string ConnectionString { get; }
}