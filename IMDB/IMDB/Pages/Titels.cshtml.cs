using System.Collections;
using System.Data;
using System.Net.Mime;
using System.Text.Json;
using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Pages.Database;
using WebApplication2.Pages.Database.model;

namespace WebApplication2.Pages;

public class Titels : PageModel
{
    public int[] tEndYx;
    public int[] tEndYy;
    public int[] tStartYx;
    public int[] tStartYy;
    public double[] F1829x { get; set; }
    public int[] F1829y { get; set; }
    public double[] M1829x { get; set; }
    public int[] M1829y { get; set; }
    public double[] MSUB18x { get; set; }
    public int[] MSUB18y { get; set; }
    public double[] FSUB18x { get; set; }
    public int[] FSUB18y { get; set; }
    public int[] cSnrx { get; set; }
    public int[] cSnry { get; set; }
    public double[] tAvgx { get; set; }
    public int[] tAvgy { get; set; }
    
    public void OnGet()
    {
        IEnumerable<demographic> demographics = getDemographics("female_18_29");
        F1829x = demographics.Select(x => Math.Round(x.rating, 1)).ToArray();
        F1829y = demographics.Select(x => x.count).ToArray();
        
        demographics = getDemographics("male_18_29");
        M1829x = demographics.Select(x => Math.Round(x.rating, 1)).ToArray();
        M1829y = demographics.Select(x => x.count).ToArray();
        
        demographics = getDemographics("male_under_18");
        MSUB18x = demographics.Select(x => Math.Round(x.rating, 1)).ToArray();
        MSUB18y = demographics.Select(x => x.count).ToArray();
        
        demographics = getDemographics("female_under_18");
        FSUB18x = demographics.Select(x => Math.Round(x.rating, 1)).ToArray();
        FSUB18y = demographics.Select(x => x.count).ToArray();

        IEnumerable<titleInfo> titleInfos = getTifleInfo();
        tAvgx = titleInfos.Select(x => Math.Round(x.rating, 1)).ToArray();
        tAvgy = titleInfos.Select(x => x.count).ToArray();

        IEnumerable<seasonNr> seasonNrs = getSeasonNrs();
        cSnrx = seasonNrs.Select(x => x.number).ToArray();
        cSnry = seasonNrs.Select(x => x.count).ToArray();

        IEnumerable<year> start = getStartYears();
        tStartYx = start.Select(x => x.startyear).ToArray();
        tStartYy = start.Select(x => x.count).ToArray();
        
        IEnumerable<year> end = getEndYears();
        tEndYx = end.Select(x => x.endyear).ToArray();
        tEndYy = end.Select(x => x.count).ToArray();
    }
    
    private IDbConnection getConnection()
    {
        return new DbUtils().Connect();
    }
    
    public IEnumerable<demographic> getDemographics(string category)
    {
        string ratings = category + "_rating";
        string numvotes = category + "_numvotes";
        
        string sql = $"SELECT DISTINCT {ratings} AS rating, SUM({numvotes}) AS count " +
                     "FROM demographics " +
                     $"GROUP BY {ratings}";


        using var connection = getConnection();
        var demographics = connection.Query<demographic>(sql);

        return demographics;
    }

    public IEnumerable<titleInfo> getTifleInfo()
    {
        string sql = @"SELECT DISTINCT averagerating AS rating, COUNT(tconst) AS count FROM title GROUP BY averagerating";
        
        using var connection = getConnection();
        var titleInfos = connection.Query<titleInfo>(sql);

        return titleInfos;
    }

    public IEnumerable<seasonNr> getSeasonNrs()
    {
        string sql = @"SELECT DISTINCT seasonnr AS number, count(tconst) AS count FROM title GROUP BY seasonnr";
        
        using var connection = getConnection();
        var seasonNrs = connection.Query<seasonNr>(sql);

        return seasonNrs;
    }

    public IEnumerable<year> getStartYears()
    {
        string sql = @"SELECT DISTINCT startyear AS startyear, SUM(numvotes) AS count FROM title WHERE startyear is not null  GROUP BY startyear";
        
        using var connection = getConnection();
        var years = connection.Query<year>(sql);

        return years;
    }
    
    public IEnumerable<year> getEndYears()
    {
        string sql = @"SELECT DISTINCT endyear AS endyear, SUM(numvotes) AS count FROM title WHERE endyear is not null GROUP BY endyear";
        
        using var connection = getConnection();
        var years = connection.Query<year>(sql);

        return years;
    }
}

public class demographic
{
    public double rating { get; set; }
    public int count { get; set; }
}

public class titleInfo
{
    public double rating { get; set; }
    public int count { get; set; }
}

public class year
{
    public int count { get; set; }
    public int startyear { get; set; }
    public int endyear { get; set; }
}

public class seasonNr
{
    public int number { get; set; }
    public int count { get; set; }
}
