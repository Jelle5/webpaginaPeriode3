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
        string sql = @"SELECT * FROM title WHERE numvotes > 5000 order by random() LIMIT 100";

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
    
    public IEnumerable<Title> settings(Settings settings)
    {
         //string isAdultR;
         //string isstartR;
         string startyearR;
         //string isendR;
         int endyearR;
         //string isruntimeR;
         int runtimeMinutesR;
         //string isavgR;
         double averageRatingR;
         //string isnumR;
         int numVotesR;
         //string isseasonR;
         int seasonnrR;
         int episodenrR;
        
         var sqlPredicates = new List<string>();
         var queryParams = new DynamicParameters();
         
         if (settings.type != "empty")
         {
             sqlPredicates.Add("averagerating = @param0");
             queryParams.Add("param0", settings.type, System.Data.DbType.String);
         }
         
        if (settings.startyear != 0)
        {
            sqlPredicates.Add("startyear = @param1");
            queryParams.Add("param1", settings.startyear, System.Data.DbType.Int16);
        }
        
        if (settings.endyear != 0)
        {
            sqlPredicates.Add("endyear = @param2");
            queryParams.Add("param2", settings.endyear, System.Data.DbType.Int16);
        }
        
        if (settings.runtimeMinutes != 0)
        {
            sqlPredicates.Add("runtimemin = @param3");
            queryParams.Add("param3", settings.runtimeMinutes, System.Data.DbType.Int16);
        }
        
        if (settings.averageRating != 0)
        {
            sqlPredicates.Add("averagerating = @param4");
            queryParams.Add("param4", settings.averageRating, System.Data.DbType.Double);
        }
        
        if (settings.numVotes != 0)
        {
            sqlPredicates.Add("numvotes = @param5");
            queryParams.Add("param5", settings.numVotes, System.Data.DbType.Int16);
        }
        
        if (settings.episodenr != 0)
        {
            sqlPredicates.Add("episodenr = @param6");
            queryParams.Add("param6", settings.episodenr, System.Data.DbType.Int16);
        }
        
        if (settings.seasonnr != 0)
        {
            sqlPredicates.Add("seasonnr = @param7");
            queryParams.Add("param7", settings.seasonnr, System.Data.DbType.Int16);
        }

        
        string sql = @"SELECT * FROM Title ";

        foreach (var param in sqlPredicates)
        {
            if (sqlPredicates.ElementAt(0) == param)
            {
                sql = sql + "WHERE " + param;
            }
            else
            {
                sql = sql + " AND " + param;
            }
        }
        
        using var connection = getConnection();
        var title = connection.Query<Title>(sql, queryParams);

        return title;
    }
}