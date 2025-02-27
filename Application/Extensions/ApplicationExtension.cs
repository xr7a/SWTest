using Application.Dto.Requests;
using Application.Dto.Requests.Passport;
using Application.Interfaces;
using Application.Services;
using Domain.DbModels;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ApplicationExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        return services;
    }

    public static IServiceProvider AddMapper(this IServiceProvider services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        TypeAdapterConfig<CreateEmployeeRequest, DbEmployeePassport>
            .NewConfig()
            .Map(dest => dest.PassportType, src => src.Passport.Type)
            .Map(dest => dest.PassportNumber, src => src.Passport.Number)
            .IgnoreNullValues(true);
        TypeAdapterConfig<CreatePassportRequest, DbPassport>
            .NewConfig()
            .IgnoreNullValues(true);
        return services;
    }
}