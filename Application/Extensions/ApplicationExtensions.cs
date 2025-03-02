using Application.Dto.Requests;
using Application.Dto.Requests.Employee;
using Application.Dto.Requests.Passport;
using Application.Dto.Responses;
using Application.Dto.Responses.Department;
using Application.Dto.Responses.Employee;
using Application.Interfaces;
using Application.Services;
using Domain.DbModels;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        return services;
    }

    public static IServiceProvider AddMapper(this IServiceProvider services)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        TypeAdapterConfig<UpdateEmployeeRequest, DbEmployee>
            .NewConfig()
            .IgnoreNullValues(true);


        TypeAdapterConfig<DbEmployeeFull, GetEmployeesByDepartmentResponse>
            .NewConfig()
            .Map(dest => dest.Passport, src => new GetPassportResponse
            {
                Number = src.PassportNumber,
                Type = src.PassportType
            })
            .Map(dest => dest.Department, src => new GetDepartmentResponse
            {
                Name = src.DepartmentName,
                Phone = src.DepartmentPhone
            });

        TypeAdapterConfig<DbEmployee, UpdateEmployeeResponse>
            .NewConfig()
            .IgnoreNullValues(true);
        
        TypeAdapterConfig.GlobalSettings.Compile();


        return services;
    }
}