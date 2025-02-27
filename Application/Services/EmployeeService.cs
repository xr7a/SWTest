using Application.Dto.Requests;
using Application.Dto.Responses;
using Application.Exceptions;
using Application.Exceptions.Department;
using Application.Exceptions.Employee;
using Application.Interfaces;
using Domain.DbModels;
using Domain.Interfaces;
using Mapster;

namespace Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPassportRepository _passportRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public EmployeeService(IEmployeeRepository employeeRepository, IPassportRepository passportRepository,
        IDepartmentRepository departmentRepository)
    {
        _employeeRepository = employeeRepository;
        _passportRepository = passportRepository;
        _departmentRepository = departmentRepository;
    }

    public async Task<CreateEmployeeResponse> CreateEmployee(CreateEmployeeRequest employeeRequest)
    {
        var isEmployeeExist = await _employeeRepository.IsEmployeeExistByPhone(employeeRequest.Phone);
        if (isEmployeeExist)
        {
            throw new EmployeeAlreadyExistException(employeeRequest.Phone);
        }
        
        var isDepartmentExist = await _departmentRepository.IsDepartmentExistById(employeeRequest.DepartmentId);
        if (!isDepartmentExist)
        {
            throw new DepartmentDoesNotExistException(employeeRequest.DepartmentId);
        }

        _employeeRepository.BeginTransaction();
        var employee = employeeRequest.Adapt<DbEmployee>();
        try
        {
            var id = await _employeeRepository.CreateEmployee(employee);
            var passport = employeeRequest.Passport.Adapt<DbPassport>();
            passport.EmployeeId = id;
            await _passportRepository.CreatePassport(passport);

            _employeeRepository.Commit();
            return new CreateEmployeeResponse() { Id = id };
        }
        catch
        {
            _employeeRepository.Rollback();
            throw;
        }
    }

    public async Task DeleteEmployee(int id)
    {
        var isEmployeeExist = await _employeeRepository.IsEmployeeExistById(id);
        if (!isEmployeeExist)
        {
            throw new EmployeeDoesNotExistException(id);
        }

        await _employeeRepository.DeleteEmployee(id);
    }
}