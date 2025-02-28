using Application.Dto.Requests;
using Application.Dto.Requests.Employee;
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
        if (!await _employeeRepository.IsEmployeeExistById(id))
        {
            throw new EmployeeDoesNotExistException(id);
        }

        bool employeeHasUpdates = false;
        bool passportHasUpdates = updateEmployeeRequest.Passport != null &&
                                  (!string.IsNullOrEmpty(updateEmployeeRequest.Passport.Type) ||
                                   !string.IsNullOrEmpty(updateEmployeeRequest.Passport.Number));

        if (!string.IsNullOrEmpty(updateEmployeeRequest.Phone))
        {
            if (await _employeeRepository.IsEmployeeExistByPhone(updateEmployeeRequest.Phone))
            {
                throw new PhoneIsAlreadyUsedException(updateEmployeeRequest.Phone);
            }

            employeeHasUpdates = true;
        }

        if (updateEmployeeRequest.DepartmentId.HasValue)
        {
            if (!await _departmentRepository.IsDepartmentExistById(updateEmployeeRequest.DepartmentId.Value))
            {
                throw new DepartmentDoesNotExistException(updateEmployeeRequest.DepartmentId.Value);
            }

            employeeHasUpdates = true;
        }

        if (!string.IsNullOrEmpty(updateEmployeeRequest.Name) ||
            !string.IsNullOrEmpty(updateEmployeeRequest.Surname) ||
            updateEmployeeRequest.CompanyId.HasValue)
        {
            employeeHasUpdates = true;
        }

        if (employeeHasUpdates && passportHasUpdates)
        {
            _employeeRepository.BeginTransaction();
            try
            {
                var employee = updateEmployeeRequest.Adapt<DbEmployee>();
                employee.Id = id;
                var passport = updateEmployeeRequest.Passport.Adapt<DbPassport>();
                passport.EmployeeId = id;
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
        else if (employeeHasUpdates)
        {
            var employee = updateEmployeeRequest.Adapt<DbEmployee>();
            employee.Id = id;
            await _employeeRepository.UpdateEmployee(employee);
        }
        else if (passportHasUpdates)
        {
            var passport = updateEmployeeRequest.Passport.Adapt<DbPassport>();
            passport.EmployeeId = id;
            await _passportRepository.UpdatePassport(passport);
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

    public async Task DeleteEmployee(int id)
    {
        var isEmployeeExist = await _employeeRepository.IsEmployeeExistById(id);
        if (isEmployeeExist)
        {
            throw new EmployeeDoesNotExistException(id);
        }

        await _employeeRepository.DeleteEmployee(id);
    }
}