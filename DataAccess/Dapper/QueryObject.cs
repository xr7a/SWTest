using DataAccess.Dapper.Interfaces;

namespace DataAccess.Dapper;

public class QueryObject : IQueryObject
{
    public QueryObject(string sql, object parameters)
    {
        if (string.IsNullOrEmpty(sql))
        {
            throw new ArgumentException("sql is empty");
        }

        Sql = sql;
        Params = parameters;
    }
    
    public string Sql { get; set; }
    public object Params { get; set; }
}