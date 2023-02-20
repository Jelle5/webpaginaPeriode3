using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
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

    public IEnumerable<individual> getByTconst(string Tconst)
    {
        string sql = @"SELECT i.* FROM individual i inner join principals p on i.nconst = p.nconst
                        WHERE tconst = @Tconst";
        using var connection = getConnection();
        //var parameters = new {Tconst = Tconst };
        var individual = connection.Query<individual>(sql, new{Tconst = Tconst});
        return individual;
    }

}