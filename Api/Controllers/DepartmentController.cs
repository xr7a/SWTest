using Application.Dto.Requests.Department;
using Application.Dto.Responses.Department;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/departments")]
public class DepartmentController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IEmployeeService employeeService, IDepartmentService departmentService)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
    }

    [HttpGet("{id}/employees")]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await _employeeService.GetEmployeesByDepartmentId(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentRequest departmentRequest)
    {
        return Ok(await _departmentService.CreateAsync(departmentRequest));
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] UpdateDepartmentRequest request, int id)
    {
        await _departmentService.UpdateAsync(request, id);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        await _departmentService.DeleteAsync(id);
        return Ok();
    }
}