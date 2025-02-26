using DataAccess.Dapper;
using DataAccess.Dapper.Interfaces;
using Domain.DbModels;
using Domain.Interfaces;

namespace Infrastructure.Repositories;

public class EmployeeRepository(IDapperContext dapperContext) : IEmployeeRepository
{
    public void BeginTransaction()
    {
        dapperContext.BeginTransaction();
    }

    public async Task CreateAsync()
    {
        
    }

    public async Task<DbEmployee?> GetEmployeeByIdAsync(int id)
    {
        return await dapperContext.FirstOrDefault<DbEmployee>(new QueryObject(
            @"select * from employees where id = @id", new { id }));
    }

    public async Task<List<DbEmployeeFull>?> GetEmployeesByCompanyAsync(int companyId)
    {
        return await dapperContext.ListOrEmpty<DbEmployeeFull>(new QueryObject(
            @"
            select 
                e.id, e.name, e.surname, e.phone, e.company_id,
                p.type as passport_type, p.number as passport_number,
                d.name as department_name, d.phone as department_phone
            from employees
            left join passports p on e.id = p.employee_id
            left join departments d on e.department_id = d.id
            where e.company_id = @companyId", new { companyId }));
    }

    public async Task<List<DbEmployeeFull>?> GetEmployeesByDepartmentAsync(int departmentId)
    {
        return await dapperContext.ListOrEmpty<DbEmployeeFull>(new QueryObject(
            @"
            select
                e.id, e.name, e.surname, e.phone, e.company_id,
                p.type as passport_type, p.number as passport_number,
                d.name as department_name, d.phone as department_phone
            from employees e
            left join passports p on e.id = p.employee_id
            left join departments d on e.departmentId = d.id
            where e.id = @departmentId;", new { departmentId }));
    }
}