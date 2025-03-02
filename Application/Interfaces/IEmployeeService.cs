using Application.Dto.Requests;
using Application.Dto.Requests.Employee;
using Application.Dto.Responses;
using Application.Dto.Responses.Employee;

namespace Application.Interfaces;

public interface IEmployeeService
{
    public Task<List<GetEmployeesByDepartmentResponse>> GetEmployeesByDepartmentId(int id);
    public Task<List<GetEmployeesByDepartmentResponse>> GetEmployeesByCompanyId(int id);
    public Task<CreateEmployeeResponse> CreateEmployee(CreateEmployeeRequest employeeRequest);
    public Task<UpdateEmployeeResponse> UpdateEmployee(UpdateEmployeeRequest updateEmployeeRequest, int id);
    public Task DeleteEmployee(int id);
}