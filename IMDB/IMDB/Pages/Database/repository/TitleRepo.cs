using System.Data;
using Dapper;
using Npgsql;
using WebApplication2.Pages.Database.model;

namespace WebApplication2.Pages.Database.repository;

public class TitleRepo
{
    private IDbConnection getConnection()
    {
        return new DbUtils().Connect();
    }

    public IEnumerable<Title> getAll()
    {
        string sql = @"SELECT * FROM title order by random() LIMIT 100";

        using var connection = getConnection();
        var title = connection.Query<Title>(sql);

        return title;
    }
}