using System.Data;
using Dapper;
using DataAccess.Dapper.Interfaces;
using DataAccess.Dapper.Interfaces.Settings;
using Npgsql;

namespace DataAccess.Dapper;

public class DapperContext(IDapperSettings dapperSettings)
    : IDapperContext, IDisposable
{
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;

    public void BeginTransaction()
    {
        _connection = new NpgsqlConnection(dapperSettings.ConnectionString);
        if (_connection.State != ConnectionState.Open)
        {
            _connection.Open();
        }

        _transaction = _connection.BeginTransaction();
    }

    public void Commit()
    {
        _transaction?.Commit();
        Dispose();
    }

    public void Rollback()
    {
        _transaction?.Rollback();
        Dispose();
    }

    public async Task<T?> FirstOrDefault<T>(IQueryObject queryObject)
    {
        return await Execute(query =>
                query.QueryFirstOrDefaultAsync<T>(queryObject.Sql, queryObject.Params))
            .ConfigureAwait(false);
    }

    public async Task<List<T>?> ListOrEmpty<T>(IQueryObject queryObject)
    {
        return (await Execute(query =>
                    query.QueryAsync<T>(queryObject.Sql, queryObject.Params))
                .ConfigureAwait(false))
            .ToList();
    }

    public async Task<T> CommandWithResponse<T>(IQueryObject queryObject)
    {
        return await Execute(query =>
                query.QueryFirstAsync<T>(queryObject.Sql, queryObject.Params,
                    transaction: _transaction))
            .ConfigureAwait(false);
    }

    public async Task Command(IQueryObject queryObject)
    {
        await Execute(query =>
                query.ExecuteAsync(queryObject.Sql, queryObject.Params,
                    transaction: _transaction))
            .ConfigureAwait(false);
    }

    private async Task<T> Execute<T>(Func<IDbConnection, Task<T>> query)
    {
        if (_transaction != null && _connection != null)
        {
            return await query(_connection).ConfigureAwait(false);
        }

        await using var executeConnection = new NpgsqlConnection(dapperSettings.ConnectionString);
        var result = await query(executeConnection).ConfigureAwait(false);
        await executeConnection.CloseAsync();
        return result;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _transaction = null;
        _connection?.Close();
        _connection?.Dispose();
        _connection = null;
    }
}