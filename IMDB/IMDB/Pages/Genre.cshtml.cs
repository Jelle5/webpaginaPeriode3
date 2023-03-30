
using System.Data;
using System.Text.Json;
using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Pages.Database;
using WebApplication2.Pages.Database.model;

namespace WebApplication2.Pages;

public class Genre : PageModel
{
    public string[] x { get; set; }
    public decimal[] y { get; set; }
    public int[] size { get; set; }
    
    public void OnGet()
    {
        IEnumerable<avgCountGenre> avgCountGenres = getavgCountGenre();
        x = avgCountGenres.Select(x => x.genre).ToArray();
        y = avgCountGenres.Select(x => Math.Round(x.averagerating, 1)).ToArray();
        size = avgCountGenres.Select(x => x.count / 25000).ToArray();
    }
    
    private IDbConnection getConnection()
    {
        return new DbUtils().Connect();
    }
    
    public IEnumerable<avgCountGenre> getavgCountGenre()
    {
        string sql = @"SELECT genre, count(t.tconst), CAST(avg(averagerating) AS DECIMAL(10,2)) AS averagerating
                    FROM title t
                    INNER JOIN genres gs ON gs.tconst = t.tconst
                    INNER JOIN genre g ON gs.gconst = g.gconst
                    GROUP BY genre";

        using var connection = getConnection();
        var avgCountGenre = connection.Query<avgCountGenre>(sql);

        return avgCountGenre;
    }
}

public class avgCountGenre
{
    public string genre { get; set; }
    public int count { get; set; }
    public decimal averagerating { get; set; }
}

