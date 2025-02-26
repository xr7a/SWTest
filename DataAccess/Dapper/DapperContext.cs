using System.Data;
using Dapper;
using DataAccess.Dapper.Interfaces;
using DataAccess.Dapper.Interfaces.Settings;
using Npgsql;

namespace DataAccess.Dapper;

public class DapperContext(IDapperSettings dapperSettings, IDbConnection? connection, IDbTransaction? transaction)
    : IDapperContext, IDisposable
{
    public void BeginTransaction()
    {
        connection = new NpgsqlConnection(dapperSettings.ConnectionString);
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        transaction = connection.BeginTransaction();
    }

    public void Commit()
    {
        transaction?.Commit();
        Dispose();
    }

    public void Rollback()
    {
        transaction?.Rollback();
        Dispose();
    }

    public async Task<T?> FirstOrDefault<T>(IQueryObject queryObject)
    {
        return await Execute(query =>
                query.QueryFirstOrDefaultAsync(queryObject.Sql, queryObject.Params))
            .ConfigureAwait(false);
    }

    public async Task<List<T>?> ListOrEmpty<T>(IQueryObject queryObject)
    {
        return (await Execute(query =>
                    query.QueryAsync<T>(queryObject.Sql, queryObject.Params))
                .ConfigureAwait(false))
            .ToList();
    }

    public async Task<T> CommandWithResponse<T>(IQueryObject queryObject, bool useTransaction = false)
    {
        return await Execute(query =>
                query.QueryFirstAsync<T>(queryObject.Sql, queryObject.Params,
                    transaction: useTransaction ? transaction : null))
            .ConfigureAwait(false);
    }

    public async Task Command(IQueryObject queryObject, bool useTransaction = false)
    {
        await Execute(query =>
                query.ExecuteAsync(queryObject.Sql, queryObject.Params,
                    transaction: useTransaction ? transaction : null))
            .ConfigureAwait(false);
    }
    private async Task<T> Execute<T>(Func<IDbConnection, Task<T>> query)
    {
        if (transaction != null && connection != null)
        {
            return await query(connection).ConfigureAwait(false);
        }
        await using var executeConnection = new NpgsqlConnection(dapperSettings.ConnectionString);
        var result = await query(executeConnection).ConfigureAwait(false);
        await executeConnection.CloseAsync();
        return result;
    }

    public void Dispose()
    {
        transaction?.Dispose();
        transaction = null;
        connection?.Close();
        connection?.Dispose();
        connection = null
            ;
    }
}