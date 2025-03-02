using Domain.DbModels;

namespace Domain.Interfaces;

public interface IEmployeeRepository
{
    void BeginTransaction();
    void Commit();
    void Rollback();
    public Task<int> CreateEmployee(DbEmployee dbEmployee);
    public Task<DbEmployee> UpdateEmployee(DbEmployee dbEmployee);
    public Task<bool> IsEmployeeExistByPhone(string phone);
    public Task<bool> IsEmployeeExistById(int id);
    public Task<DbEmployeePassport?> GetEmployeeWithPassport(int id);
    public Task<List<DbEmployeeFull>?> GetEmployeesByCompany(int companyId);
    public Task<List<DbEmployeeFull>?> GetEmployeesByDepartment(int departmentId);
    public Task DeleteEmployee(int id);
}