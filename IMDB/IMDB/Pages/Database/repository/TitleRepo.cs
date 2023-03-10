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
    
    public IEnumerable<Title> getAllType()
    {
        string sql = @"select DISTINCT(type) FROM title";

        using var connection = getConnection();
        var title = connection.Query<Title>(sql);

        return title;
        
    }
    
    
    public IEnumerable<Title> settings(Settings settings)
    {
        var sqlPredicates = new List<string>();
         var queryParams = new DynamicParameters();

         if (settings.type != "empty")
         {
             sqlPredicates.Add("type = @param0");
             queryParams.Add("param0", settings.type, System.Data.DbType.String);
         }
         
        if (settings.startyear != 0)
        {
            
            sqlPredicates.Add("startyear " + settings.isstart + " @param1");
            queryParams.Add("param1", settings.startyear, System.Data.DbType.Int16);
        }

        if (settings.endyear != 0)
        {
            sqlPredicates.Add("endyear " + settings.isend  + " @param2");
            queryParams.Add("param2", settings.endyear, System.Data.DbType.Int16);
        }

        if (settings.runtimeMinutes != 0)
        {
            sqlPredicates.Add("runtimemin " + settings.isruntime + " @param3");
            queryParams.Add("param3", settings.runtimeMinutes, System.Data.DbType.Int16);
        }

        if (settings.averagerating != 0)
        {
            sqlPredicates.Add("averagerating " + settings.isavg + " @param4");
            queryParams.Add("param4", settings.averagerating, System.Data.DbType.Double);
        }

        if (settings.numVotes != 0)
        {
            sqlPredicates.Add("numvotes " + settings.isnum + " @param5");
            queryParams.Add("param5", settings.numVotes, System.Data.DbType.Int16);
        }

        if (settings.episodenr != 0)
        {
            sqlPredicates.Add("episodenr = @param6");
            queryParams.Add("param6", settings.episodenr, System.Data.DbType.Int16);
        }

        if (settings.seasonnr != 0)
        {
            sqlPredicates.Add("seasonnr " + settings.isseason + " @param7");
            queryParams.Add("param7", settings.seasonnr, System.Data.DbType.Int16);
        }
        if (settings.limit != 0)
        {
            queryParams.Add("param8", settings.limit, System.Data.DbType.Int16);
        }
        else
        {
            queryParams.Add("param8", 100, System.Data.DbType.Int16);
        }


        string sql = @"SELECT * FROM Title ";

        foreach (var param in sqlPredicates)
        {
            if (sqlPredicates.ElementAt(0) == param)
                sql += "WHERE " + param;
            else
                sql += " AND " + param;
        }

        sql += " ORDER BY random() ASC LIMIT @param8";
        using var connection = getConnection();
        var title = connection.Query<Title>(sql, queryParams);

        string filePath = "csvData/DashboardTable.csv";
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            var headers = title.First().GetType().GetProperties().Select(p => p.Name);
            writer.WriteLine(string.Join(";", headers));

            foreach (var row in title)
            {
                var values = row.GetType().GetProperties().Select(p => p.GetValue(row, null));
                var valueStrings = values.Select(v => v == null ? "" : v.ToString());
                writer.WriteLine(string.Join(";", valueStrings));
            }
        }

        return title;
    }
}