using Application.Dto.Requests.Employee;
using Application.Dto.Responses;
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
            return new CreateEmployeeResponse { Id = id };
        }
        catch
        {
            _employeeRepository.Rollback();
            throw;
        }
    }

    public async Task UpdateEmployee(UpdateEmployeeRequest updateEmployeeRequest, int id)
    {
        var employeeWithPassport = await _employeeRepository.GetEmployeeWithPassport(id);
        
        if (employeeWithPassport is null)
        {
            throw new EmployeeDoesNotExistException(id);
        }

        if (updateEmployeeRequest.DepartmentId.HasValue)
        {
            var isDepartmentExist =
                await _departmentRepository.IsDepartmentExistById((int)updateEmployeeRequest.DepartmentId);
            if (!isDepartmentExist)
            {
                throw new DepartmentDoesNotExistException((int)updateEmployeeRequest.DepartmentId);
            }
        }
        
        if(!string.IsNullOrEmpty(updateEmployeeRequest.Phone))
        {
            var isEmployeeExistByPhone = await _employeeRepository.IsEmployeeExistByPhone(updateEmployeeRequest.Phone);
            if (isEmployeeExistByPhone)
            {
                throw new PhoneIsAlreadyUsedException(updateEmployeeRequest.Phone);
            }
        }
        
        var employee = new DbEmployee
        {
            Id = employeeWithPassport.Id,
            Name = updateEmployeeRequest.Name ?? employeeWithPassport.Name,
            Surname = updateEmployeeRequest.Surname ?? employeeWithPassport.Surname,
            CompanyId = updateEmployeeRequest.CompanyId ?? employeeWithPassport.CompanyId,
            DepartmentId = updateEmployeeRequest.DepartmentId ?? employeeWithPassport.DepartmentId,
            Phone = updateEmployeeRequest.Phone ?? employeeWithPassport.Phone
        };
        var passport = new DbPassport
        {
            EmployeeId = employeeWithPassport.Id,
            Number = updateEmployeeRequest.Passport?.Number ?? employeeWithPassport.PassportNumber,
            Type = updateEmployeeRequest.Passport?.Type ?? employeeWithPassport.PassportType
        };
        
        _employeeRepository.BeginTransaction();
        try
        {
            await _employeeRepository.UpdateEmployee(employee);
            await _passportRepository.UpdatePassport(passport);
            _employeeRepository.Commit();
        }
        catch
        {
            _employeeRepository.Rollback();
            throw;
        }
    }


    public async Task<List<GetEmployeesByDepartmentResponse>> GetEmployeesByDepartmentId(int id)
    {
        var isDepartmentExist = await _departmentRepository.IsDepartmentExistById(id);
        if (!isDepartmentExist)
        {
            throw new DepartmentDoesNotExistException(id);
        }

        var dbEmployees = await _employeeRepository.GetEmployeesByDepartment(id);
        return dbEmployees.Select(i => i.Adapt<GetEmployeesByDepartmentResponse>()).ToList();
    }
    public async Task<List<GetEmployeesByDepartmentResponse>> GetEmployeesByCompanyId(int id)
    {
        var dbEmployees = await _employeeRepository.GetEmployeesByCompany(id);
        return dbEmployees.Adapt<List<GetEmployeesByDepartmentResponse>>();
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