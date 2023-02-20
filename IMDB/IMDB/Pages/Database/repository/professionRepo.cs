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
    
    public IEnumerable<profession> getByNconst(string Nconst) {
        string sql = @"SELECT p.* FROM profession p 
                        INNER JOIN professions ps on p.pconst = ps.pconst
                        WHERE ps.nconst = @Nconst";

        using var connection = getConnection();
    var profession = connection.Query<profession>(sql, new{Nconst = Nconst});

        return profession;
}
}