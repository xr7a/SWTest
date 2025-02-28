using DataAccess.Dapper;
using DataAccess.Dapper.Interfaces;
using Domain.DbModels;
using Domain.Interfaces;
using Infrastructure.Repositories.Scripts;

namespace Infrastructure.Repositories;

public class DepartmentRepository(IDapperContext dapperContext) : IDepartmentRepository
{
    public async Task<bool> IsDepartmentExistById(int id)
    {
        return await dapperContext.CommandWithResponse<bool>(new QueryObject(
            Sql.IsDepartmentExistById, new { id }));
    }
    public async Task<bool> IsDepartmentExistByPhone(string phone)
    {
        return await dapperContext.CommandWithResponse<bool>(new QueryObject(
            Sql.isDepartmentExistByPhone, new { phone }));
    }

    public async Task<DbDepartment> GetDepartmentById(int id)
    {
        return await dapperContext.CommandWithResponse<DbDepartment>(new QueryObject(
            Sql.GetDepartmentById, new { id }));
    }
    
    public async Task<int> CreateDepartment(DbDepartment dbDepartment)
    {
        return await dapperContext.CommandWithResponse<int>(new QueryObject(
            Sql.CreateDepartment, new { name = dbDepartment.Name, phone = dbDepartment.Phone }));
    }

    public async Task UpdateDepartment(DbDepartment dbDepartment)
    {
        await dapperContext.Command(new QueryObject(
            Sql.UpdateDepartment, new { name = dbDepartment.Name, phone = dbDepartment.Phone }));
    }

    public async Task DeleteDepartment(int id)
    {
        await dapperContext.Command(new QueryObject(
            Sql.DeleteDepartment, new { id }));
    }
}