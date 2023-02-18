using System.Data;
using Dapper;
using WebApplication2.Pages.Database.model;

namespace WebApplication2.Pages.Database.repository;

public class genresRepo
{
    private IDbConnection getConnection()
    {
        return new DbUtils().Connect();
    }

    public IEnumerable<genres> getAll()
    {
        string sql = @"SELECT * FROM genres LIMIT 100";

        using var connection = getConnection();
        var genres = connection.Query<genres>(sql);

        return genres;
    }
}