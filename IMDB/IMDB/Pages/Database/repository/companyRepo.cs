using System.Data;
using Dapper;
using WebApplication2.Pages.Database.model;

namespace WebApplication2.Pages.Database.repository;

public class companyRepo
{
    private IDbConnection getConnection()
    {
        return new DbUtils().Connect();
    }

    public IEnumerable<company> getAll()
    {
        string sql = @"SELECT * FROM company LIMIT 100";

        using var connection = getConnection();
        var company = connection.Query<company>(sql);

        return company;
    }
    
    public IEnumerable<company> getByTconst(string Tconst)
    {
        string sql = @"SELECT c.* FROM company c 
                        INNER JOIN companies cs on c.coconst = cs.coconst 
                        WHERE cs.tconst = @Tconst";
        using var connection = getConnection();
        var company = connection.Query<company>(sql, new{Tconst = Tconst});

        return company;
    }
}