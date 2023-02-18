using System.Data;
using Dapper;
using WebApplication2.Pages.Database.model;

namespace WebApplication2.Pages.Database.repository;

public class principalsRepo
{
    private IDbConnection getConnection()
    {
        return new DbUtils().Connect();
    }

    public IEnumerable<principals> getAll()
    {
        string sql = @"SELECT * FROM principals LIMIT 100";

        using var connection = getConnection();
        var principals = connection.Query<principals>(sql);

        return principals;
    }
}