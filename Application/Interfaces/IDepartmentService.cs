using Application.Dto.Requests.Department;
using Application.Dto.Responses;
using Application.Dto.Responses.Department;

namespace Application.Interfaces;

public interface IDepartmentService
{
    public Task<CreateDepartmentResponse> CreateAsync(CreateDepartmentRequest departmentRequest);
    public Task<GetDepartmentResponse> UpdateAsync(UpdateDepartmentRequest departmentRequest, int id);
    public Task DeleteAsync(int id);
}