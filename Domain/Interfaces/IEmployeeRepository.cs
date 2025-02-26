using DataAccess.Dapper.Interfaces;

namespace Domain.Interfaces;

public interface IEmployeeRepository
{
    void BeginTransaction();
}