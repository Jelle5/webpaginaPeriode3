using System.Data;
using Dapper;
using WebApplication2.Pages.Database.model;

namespace WebApplication2.Pages.Database.repository;

public class akasRepo
{
    private IDbConnection getConnection()
    {
        return new DbUtils().Connect();
    }

    public IEnumerable<akas> getAll()
    {
        string sql = @"SELECT * FROM akas LIMIT 100";

        using var connection = getConnection();
        var akas = connection.Query<akas>(sql);

        return akas;
    }
}