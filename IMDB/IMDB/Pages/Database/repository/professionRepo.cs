using System.Data;
using Dapper;
using WebApplication2.Pages.Database.model;

namespace WebApplication2.Pages.Database.repository;

public class professionRepo
{
    private IDbConnection getConnection()
    {
        return new DbUtils().Connect();
    }

    public IEnumerable<profession> getAll()
    {
        string sql = @"SELECT * FROM profession LIMIT 100";

        using var connection = getConnection();
        var profession = connection.Query<profession>(sql);

        return profession;
    }
}