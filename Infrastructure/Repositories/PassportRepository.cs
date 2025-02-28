using DataAccess.Dapper;
using DataAccess.Dapper.Interfaces;
using Domain.DbModels;
using Domain.Interfaces;
using Infrastructure.Repositories.Scripts;

namespace Infrastructure.Repositories;

public class PassportRepository(IDapperContext dapperContext) : IPassportRepository
{
    public async Task CreatePassport(DbPassport dbPassport)
    {
        await dapperContext.Command(new QueryObject(
            Sql.CreatePassport,
            new { employeeId = dbPassport.EmployeeId, number = dbPassport.Number, type = dbPassport.Type }));
    }

    public async Task UpdatePassport(DbPassport dbPassport)
    {
        await dapperContext.Command(new QueryObject(
            Sql.UpdatePassport, new { type = dbPassport.Type, number = dbPassport.Number, employeeId = dbPassport.EmployeeId }));
    }
}