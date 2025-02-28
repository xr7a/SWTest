namespace DataAccess.Dapper.Interfaces;

public interface IDapperContext
{
    void BeginTransaction();
    void Commit();
    void Rollback();
    public Task<T?> FirstOrDefault<T>(IQueryObject queryObject);
    public Task<List<T>?> ListOrEmpty<T>(IQueryObject queryObject);
    public Task<T> CommandWithResponse<T>(IQueryObject queryObject);
    public Task Command(IQueryObject queryObject);
}