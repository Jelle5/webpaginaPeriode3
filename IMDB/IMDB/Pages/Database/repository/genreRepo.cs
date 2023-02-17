using System.Data;
using Dapper;
using WebApplication2.Pages.Database.model;

namespace WebApplication2.Pages.Database.repository;

public class genreRepo
{
    private IDbConnection getConnection()
    {
        return new DbUtils().Connect();
    }

    public IEnumerable<genre> getAll()
    {
        string sql = @"SELECT * FROM genre LIMIT 100";

        using var connection = getConnection();
        var genre = connection.Query<genre>(sql);

        return genre;
    }
}