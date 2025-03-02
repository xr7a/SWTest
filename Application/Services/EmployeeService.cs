using Application.Dto.Requests.Employee;
using Application.Dto.Responses;
using Application.Dto.Responses.Employee;
using Application.Exceptions.Department;
using Application.Exceptions.Employee;
using Application.Exceptions.Passport;
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

        var isPassportExistByNumber =
            await _passportRepository.IsPassportExistByNumber(employeeRequest.Passport.Number);
        if (isPassportExistByNumber)
        {
            throw new PassportWithNumberAlreadyExistException(employeeRequest.Passport.Number);
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

    public async Task<UpdateEmployeeResponse> UpdateEmployee(UpdateEmployeeRequest updateEmployeeRequest, int id)
    { 
        var employeeWithPassport = await _employeeRepository.GetEmployeeWithPassport(id);
        if (employeeWithPassport is null)
        {
            throw new EmployeeDoesNotExistException(id);
        }
        Console.WriteLine(employeeWithPassport.Phone);

        if (updateEmployeeRequest.DepartmentId.HasValue)
        {
            var isDepartmentExist =
                await _departmentRepository.IsDepartmentExistById((int)updateEmployeeRequest.DepartmentId);
            if (!isDepartmentExist)
            {
                throw new DepartmentDoesNotExistException((int)updateEmployeeRequest.DepartmentId);
            }
        }

        if (!string.IsNullOrEmpty(updateEmployeeRequest.Phone) &&
            updateEmployeeRequest.Phone != employeeWithPassport.Phone)
        {
            var isEmployeeExistByPhone = await _employeeRepository.IsEmployeeExistByPhone(updateEmployeeRequest.Phone);
            if (isEmployeeExistByPhone)
            {
                throw new PhoneIsAlreadyUsedException(updateEmployeeRequest.Phone);
            }
        }

        Console.WriteLine(updateEmployeeRequest.Passport?.Number);
        Console.WriteLine(employeeWithPassport.PassportNumber);
        if (!string.IsNullOrEmpty(updateEmployeeRequest.Passport?.Number) &&
            updateEmployeeRequest.Passport.Number != employeeWithPassport.PassportNumber)
        {
            var isPassportExist =
                await _passportRepository.IsPassportExistByNumber(updateEmployeeRequest.Passport.Number);
            if (isPassportExist)
            {
                throw new PassportWithNumberAlreadyExistException(updateEmployeeRequest.Passport.Number);
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
            var updatedEmployee = await _employeeRepository.UpdateEmployee(employee);
            var updatedPassport = await _passportRepository.UpdatePassport(passport);
            var updatedEmployeeWithPassport = updatedEmployee.Adapt<UpdateEmployeeResponse>();
            updatedEmployeeWithPassport.Passport = updatedPassport.Adapt<GetPassportResponse>();

            _employeeRepository.Commit();
            return updatedEmployeeWithPassport;
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
        return dbEmployees.Adapt<List<GetEmployeesByDepartmentResponse>>();
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