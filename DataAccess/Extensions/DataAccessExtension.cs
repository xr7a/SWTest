using DataAccess.Dapper;
using DataAccess.Dapper.Interfaces;
using DataAccess.Dapper.Settings;
using DataAccess.Dapper.Interfaces.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Extensions;

public static class DataAccessExtension
{
    public static IServiceCollection AddDapperContext(this IServiceCollection services)
    {
        services.AddScoped<IDapperSettings, DapperSettings>();
        services.AddScoped<IDapperContext, DapperContext>();
        return services;
    }

}