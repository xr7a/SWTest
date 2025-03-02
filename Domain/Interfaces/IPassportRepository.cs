using Domain.DbModels;

namespace Domain.Interfaces;

public interface IPassportRepository
{
    public Task CreatePassport(DbPassport dbPassport);
    public Task<DbPassport> UpdatePassport(DbPassport dbPassport);
    public Task<bool> IsPassportExistByNumber(string number);
}