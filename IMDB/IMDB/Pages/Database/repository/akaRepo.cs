using System.Data;
using Dapper;
using WebApplication2.Pages.Database.model;

namespace WebApplication2.Pages.Database.repository;

public class akaRepo
{
    private IDbConnection getConnection()
    {
        return new DbUtils().Connect();
    }

    public IEnumerable<aka> getAll()
    {
        string sql = @"SELECT * FROM aka LIMIT 100";

        using var connection = getConnection();
        var aka = connection.Query<aka>(sql);

        return aka;
    }
    
    public IEnumerable<aka> getByTconst(string Tconst)
    {
        string sql = @"SELECT a.* FROM aka a 
                        INNER JOIN akas aa on a.aconst = aa.aconst 
                        WHERE aa.tconst = @Tconst";
        using var connection = getConnection();
        var aka = connection.Query<aka>(sql, new{Tconst = Tconst});

        return aka;
    }
}