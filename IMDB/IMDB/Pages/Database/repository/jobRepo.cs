using System.Data;
using Dapper;
using WebApplication2.Pages.Database.model;

namespace WebApplication2.Pages.Database.repository;

public class jobRepo
{
    private IDbConnection getConnection()
    {
        return new DbUtils().Connect();
    }

    public IEnumerable<job> getAll()
    {
        string sql = @"SELECT * FROM job LIMIT 100";

        using var connection = getConnection();
        var job = connection.Query<job>(sql);

        return job;
    }
}