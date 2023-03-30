using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Pages.Database.model;

namespace WebApplication2.Pages;

public class Titels : PageModel
{
    public double[] tEndYx;
    public int[] tEndYy;
    public double[] tStartYx;
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
        // Load your data here
        string json = System.IO.File.ReadAllText("GraphData/female1829AllRatings.json");
        var data = JsonSerializer.Deserialize<List<Ratings>>(json);

        // Convert to arrays
        F1829x = data.Select(x => x.x).ToArray();
        F1829y = data.Select(x => x.y).ToArray();

        json = System.IO.File.ReadAllText("GraphData/female1829AllRatings.json");
        data = JsonSerializer.Deserialize<List<Ratings>>(json);

        M1829x = data.Select(x => x.x).ToArray();
        M1829y = data.Select(x => x.y).ToArray();

        json = System.IO.File.ReadAllText("GraphData/maleUnder18AllRatings.json");
        data = JsonSerializer.Deserialize<List<Ratings>>(json);

        MSUB18x = data.Select(x => x.x).ToArray();
        MSUB18y = data.Select(x => x.y).ToArray();
        
        json = System.IO.File.ReadAllText("GraphData/femaleUnder18AllRatings.json");
        data = JsonSerializer.Deserialize<List<Ratings>>(json);

        FSUB18x = data.Select(x => x.x).ToArray();
        FSUB18y = data.Select(x => x.y).ToArray();
        
        json = System.IO.File.ReadAllText("GraphData/titleAvgRating.json");
        data = JsonSerializer.Deserialize<List<Ratings>>(json);

        tAvgx = data.Select(x => x.x).ToArray();
        tAvgy = data.Select(x => x.y).ToArray();
        
        json = System.IO.File.ReadAllText("GraphData/titleEndYear.json");
        data = JsonSerializer.Deserialize<List<Ratings>>(json);

        tEndYx = data.Select(x => x.x).ToArray();
        tEndYy = data.Select(x => x.y).ToArray();
        
        json = System.IO.File.ReadAllText("GraphData/titleStartYear.json");
        data = JsonSerializer.Deserialize<List<Ratings>>(json);

        tStartYx = data.Select(x => x.x).ToArray();
        tStartYy = data.Select(x => x.y).ToArray();
        
        json = System.IO.File.ReadAllText("GraphData/countSeasonNr.json");
        var data2 = JsonSerializer.Deserialize<List<cSnr>>(json);
        
        cSnrx = data2.Select(x => x.x).ToArray();
        cSnry = data2.Select(x => x.y).ToArray();
    }
}

public class Ratings
{
    public double x { get; set; }
    public int y { get; set; }
}

public class cSnr
{
    public int x { get; set; }
    public int y { get; set; }
}


