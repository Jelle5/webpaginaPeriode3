using System.Data;
using Dapper;
using WebApplication2.Pages.Database.model;

namespace WebApplication2.Pages.Database.repository;

public class jobsRepo
{
    private IDbConnection getConnection()
    {
        return new DbUtils().Connect();
    }

    public IEnumerable<jobs> getAll()
    {
        string sql = @"SELECT * FROM jobs LIMIT 100";

        using var connection = getConnection();
        var jobs = connection.Query<jobs>(sql);

        return jobs;
    }
}