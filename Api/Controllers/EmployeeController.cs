﻿using Application.Dto.Requests;
using Application.Dto.Requests.Employee;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeeController: ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }
    
    [HttpGet("company/{id}")]
    public async Task<IActionResult> GetAll(int id)
    {
        return Ok(await _employeeService.GetEmployeesByCompanyId(id));
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody]CreateEmployeeRequest employeeRequest)
    {
        return Ok(await _employeeService.CreateEmployee(employeeRequest));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch([FromBody]UpdateEmployeeRequest updateEmployeeRequest, int id)
    {
        return Ok(await _employeeService.UpdateEmployee(updateEmployeeRequest, id));
    } 

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _employeeService.DeleteEmployee(id);
        return NoContent();
    }
}