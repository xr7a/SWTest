namespace DataAccess.Dapper.Interfaces;

public interface IDapperContext
{
    void BeginTransaction();
    public Task<T?> FirstOrDefault<T>(IQueryObject queryObject);
    public Task<List<T>?> ListOrEmpty<T>(IQueryObject queryObject);
    public Task<T> CommandWithResponse<T>(IQueryObject queryObject, bool useTransaction = false);
    public Task Command(IQueryObject queryObject, bool useTransaction = false);
}