using System.Data;
using Dapper;
using WebApplication2.Pages.Database.model;

namespace WebApplication2.Pages.Database.repository;

public class predictRepo
{
    private IDbConnection getConnection()
    {
        return new DbUtils().Connect();
    }

    public IEnumerable<double> getPred(string Genre)
    {
        string sql = @"SELECT averagerating FROM title t 
                        JOIN genres gs ON t.tconst = gs.tconst 
                        JOIN genre g ON gs.gconst = g.gconst
                        WHERE startyear > 2018 AND startyear < 2023 AND averagerating IS NOT NULL AND numvotes > 5000 AND genre LIKE @Genre";

        using var connection = getConnection();
        var predlist = connection.Query<double>(sql, new{ Genre = Genre });

        return predlist;
    }
}