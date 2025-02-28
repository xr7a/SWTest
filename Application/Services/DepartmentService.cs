using Application.Dto.Requests.Department;
using Application.Dto.Responses;
using Application.Dto.Responses.Department;
using Application.Exceptions.Department;
using Application.Interfaces;
using Domain.DbModels;
using Domain.Interfaces;
using Mapster;

namespace Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<CreateDepartmentResponse> CreateAsync(CreateDepartmentRequest departmentRequest)
    {
        var isDepartmentExist = await _departmentRepository.IsDepartmentExistByPhone(departmentRequest.Phone);
        if (isDepartmentExist)
        {
            throw new DepartmentAlreadyExistException(departmentRequest.Phone);
        }

        var department = departmentRequest.Adapt<DbDepartment>();
        var id = await _departmentRepository.CreateDepartment(department);
        return new CreateDepartmentResponse { Id = id };
    }

    public async Task UpdateAsync(UpdateDepartmentRequest departmentRequest, int id)
    {
        var department = await _departmentRepository.GetDepartmentById(id);
        if (department == null)
        {
            throw new DepartmentDoesNotExistException(id);
        }

        var updateDepartment = new DbDepartment
        {
            Id = department.Id,
            Name = departmentRequest.Name ?? department.Name,
            Phone = departmentRequest.Phone ?? department.Phone
        };
        await _departmentRepository.UpdateDepartment(updateDepartment);
    }

    public async Task DeleteAsync(int id)
    {
        var isDepartmentExist = await _departmentRepository.IsDepartmentExistById(id);
        if (isDepartmentExist)
        {
            throw new DepartmentDoesNotExistException(id);
        }

        await _departmentRepository.DeleteDepartment(id);
    }
}