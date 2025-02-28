using Application.Dto.Requests;
using Application.Dto.Requests.Employee;
using Application.Dto.Requests.Passport;
using Application.Dto.Responses;
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
        
        TypeAdapterConfig<UpdateEmployeeRequest, DbEmployee>
            .NewConfig()
            .IgnoreNullValues(true);
        
        TypeAdapterConfig<UpdatePassportRequest, DbPassport>
            .NewConfig()
            .Map(dest => dest.Number, src => src.Number)
            .Map(dest => dest.Type, src => src.Type)
            .IgnoreNullValues(true);

        TypeAdapterConfig<DbEmployeeFull, GetEmployeesByDepartmentResponse>
            .NewConfig()
            .ConstructUsing(src => new GetEmployeesByDepartmentResponse
            {
                Passport = new GetPassportResponse(),
                Department = new GetDepartmentResponse()
            })
            .Map(dest => dest.Passport.Number, src => src.PassportNumber)
            .Map(dest => dest.Passport.Type, src => src.PassportType)
            .Map(dest => dest.Department.Name, src => src.DepartmentName)
            .Map(dest => dest.Department.Phone, src => src.DepartmentPhone);

        return services;
    }
}