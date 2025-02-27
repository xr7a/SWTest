using DataAccess.Dapper;
using DataAccess.Dapper.Interfaces;
using Domain.DbModels;
using Domain.Interfaces;
using Infrastructure.Repositories.Scripts;

namespace Infrastructure.Repositories;

public class EmployeeRepository(IDapperContext dapperContext) : IEmployeeRepository
{
    public void BeginTransaction()
    {
        dapperContext.BeginTransaction();
    }

    public void Commit()
    {
        dapperContext.Commit();
    }

    public void Rollback()
    {
        dapperContext.Rollback();
    }

    public async Task<int> CreateEmployee(DbEmployee dbEmployee)
    {
        return await dapperContext.CommandWithResponse<int>(new QueryObject(
            Sql.CreateEmployee,
            new
            {
                name = dbEmployee.Name, surname = dbEmployee.Surname, phone = dbEmployee.Phone,
                companyId = dbEmployee.CompanyId, departmentId = dbEmployee.DepartmentId
            }));
    }

    public async Task UpdateEmployee(DbEmployee dbEmployee)
    {
        await dapperContext.Command(new QueryObject(
            Sql.UpdateEmployee,
            new
            {
                name = dbEmployee.Name, surname = dbEmployee.Surname, phone = dbEmployee.Phone,
                departmentId = dbEmployee.DepartmentId, companyId = dbEmployee.CompanyId
            }));
    }

    public async Task<bool> IsEmployeeExistByPhone(string phone)
    {
        return await dapperContext.CommandWithResponse<bool>(new QueryObject(
            Sql.IsEmployeeExistByPhone, new { phone }));
    } 
    public async Task<bool> IsEmployeeExistById(int id)
    {
        return await dapperContext.CommandWithResponse<bool>(new QueryObject(
            Sql.IsEmployeeExistById, new { id }));
    }

    public async Task<List<DbEmployeeFull>?> GetEmployeesByCompany(int companyId)
    {
        return await dapperContext.ListOrEmpty<DbEmployeeFull>(new QueryObject(
            Sql.GetEmployeesByCompany, new { companyId }));
    }

    public async Task<List<DbEmployeeFull>?> GetEmployeesByDepartment(int departmentId)
    {
        return await dapperContext.ListOrEmpty<DbEmployeeFull>(new QueryObject(
            Sql.GetEmployeesByDepartment, new { departmentId }));
    }

    public async Task DeleteEmployee(int id)
    {
        await dapperContext.Command(new QueryObject(
            Sql.DeleteEmployee, new { id }));
    }
}