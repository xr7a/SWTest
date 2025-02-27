using Domain.DbModels;

namespace Domain.Interfaces;

public interface IDepartmentRepository
{
    public Task<bool> IsDepartmentExistById(int id);
    public Task CreateDepartment(DbDepartment dbDepartment);
    public Task UpdateDepartment(DbDepartment dbDepartment);
    public Task DeleteDepartment(int id);
}