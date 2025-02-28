using Application.Dto.Requests;
using Application.Dto.Requests.Employee;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController: ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet("department/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        return Ok(await _employeeService.GetEmployeesByDepartmentId(id));
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody]CreateEmployeeRequest employeeRequest)
    {
        return Ok(await _employeeService.CreateEmployee(employeeRequest));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(UpdateEmployeeRequest updateEmployeeRequest, int id)
    {
        await _employeeService.UpdateEmployee(updateEmployeeRequest, id);
        return Ok();
    } 

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        await _employeeService.DeleteEmployee(id);
        return NoContent();
    }
}