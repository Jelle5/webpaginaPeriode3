using System.Data;
using Dapper;
using WebApplication2.Pages.Database.model;

namespace WebApplication2.Pages.Database.repository;

public class individualRepo
{
    private IDbConnection getConnection()
    {
        return new DbUtils().Connect();
    }

    public IEnumerable<individual> getAll()
    {
        string sql = @"SELECT * FROM individual LIMIT 100";

        using var connection = getConnection();
        var individual = connection.Query<individual>(sql);

        return individual;
    }
}