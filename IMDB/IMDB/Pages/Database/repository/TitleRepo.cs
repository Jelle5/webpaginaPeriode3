using System.Collections;
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
    
    public IEnumerable<Title> getTitle(string Tconst)
    {
        string sql = @"SELECT * FROM title WHERE tconst = @Tconst";

        using var connection = getConnection();
        var title = connection.Query<Title>(sql, new{ Tconst = Tconst });

        return title;
    }

    public IEnumerable<Title> getByNconst(string Nconst)
    {
        string sql = @"SELECT t.*, p.* FROM title t 
                        INNER JOIN principals p on t.tconst = p.tconst
                        WHERE Nconst = @Nconst";

            using var connection = getConnection();
            var title = connection.Query<Title, principals, Title>(sql, map: (t, p) =>
            {
                t.Principals = p;
                return t;
            }, splitOn:"nconst", param: new { Nconst = Nconst });

            return title;
        
    }
}