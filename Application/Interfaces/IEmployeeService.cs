using Application.Dto.Requests;
using Application.Dto.Responses;

namespace Application.Interfaces;

public interface IEmployeeService
{
    public Task<CreateEmployeeResponse> CreateEmployee(CreateEmployeeRequest employeeRequest);
    public Task DeleteEmployee(int id);
}