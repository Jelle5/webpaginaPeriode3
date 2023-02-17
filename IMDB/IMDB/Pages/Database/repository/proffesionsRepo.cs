using System.Data;
using Dapper;
using WebApplication2.Pages.Database.model;

namespace WebApplication2.Pages.Database.repository;

public class proffesionsRepo
{
    private IDbConnection getConnection()
    {
        return new DbUtils().Connect();
    }

    public IEnumerable<professions> getAll()
    {
        string sql = @"SELECT * FROM professions LIMIT 100";

        using var connection = getConnection();
        var professions = connection.Query<professions>(sql);

        return professions;
    }
}