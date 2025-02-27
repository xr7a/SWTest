using Domain.DbModels;

namespace Domain.Interfaces;

public interface IPassportRepository
{
    public Task CreatePassport(DbPassport dbPassport);
    public Task UpdatePassport(DbPassport dbPassport);
}