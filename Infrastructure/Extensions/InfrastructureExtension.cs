using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class InfrastructureExtension
{
    public static IServiceCollection AddFluentMigrator(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddFluentMigratorCore().ConfigureRunner(rb =>
                rb.AddPostgres()
                    .WithGlobalConnectionString(configuration.GetConnectionString("Database"))
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
        return services;
    }

    public static IServiceProvider UseMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetService<IMigrationRunner>();
        runner.MigrateUp();
        return serviceProvider;
    }
}