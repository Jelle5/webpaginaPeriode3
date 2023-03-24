using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication2.Pages;

public class Titels : PageModel
{
    public double[] F1829Ratings { get; set; }
    public int[] F1829Aantal { get; set; }
    public double[] M1829Ratings { get; set; }
    public int[] M1829Aantal { get; set; }
    public double[] MSUB18Ratings { get; set; }
    public int[] MSUB18Aantal { get; set; }
    public double[] FSUB18Ratings { get; set; }
    public int[] FSUB18Aantal { get; set; }
    public void OnGet()
    {
        // Load your data here
        string json = System.IO.File.ReadAllText("GraphData/female1829AllRatings.json");
        var data = JsonSerializer.Deserialize<List<Ratings>>(json);
        
        // Remove null values
        data.RemoveAll(x => x.Rating == null || x.Aantal == null || x.Rating.Value == null || x.Aantal.Value == null);

        // Convert to arrays
        F1829Ratings = data.Select(x => x.Rating.Value).ToArray();
        F1829Aantal = data.Select(x => x.Aantal.Value).ToArray();
        
        // Load your data here
        json = System.IO.File.ReadAllText("GraphData/female1829AllRatings.json");
        data = JsonSerializer.Deserialize<List<Ratings>>(json);
        
        // Remove null values
        data.RemoveAll(x => x.Rating == null || x.Aantal == null || x.Rating.Value == null || x.Aantal.Value == null);

        // Convert to arrays
        M1829Ratings = data.Select(x => x.Rating.Value).ToArray();
        M1829Aantal = data.Select(x => x.Aantal.Value).ToArray();
        
        // Load your data here
        json = System.IO.File.ReadAllText("GraphData/maleUnder18AllRatings.json");
        data = JsonSerializer.Deserialize<List<Ratings>>(json);
        
        // Remove null values
        data.RemoveAll(x => x.Rating == null || x.Aantal == null || x.Rating.Value == null || x.Aantal.Value == null);

        // Convert to arrays
        MSUB18Ratings = data.Select(x => x.Rating.Value).ToArray();
        MSUB18Aantal = data.Select(x => x.Aantal.Value).ToArray();
        
        // Load your data here
        json = System.IO.File.ReadAllText("GraphData/femaleUnder18AllRatings.json");
        data = JsonSerializer.Deserialize<List<Ratings>>(json);
        
        // Remove null values
        data.RemoveAll(x => x.Rating == null || x.Aantal == null || x.Rating.Value == null || x.Aantal.Value == null);

        // Convert to arrays
        FSUB18Ratings = data.Select(x => x.Rating.Value).ToArray();
        FSUB18Aantal = data.Select(x => x.Aantal.Value).ToArray();
    }
}

public class Ratings
{
    public double? Rating { get; set; }
    public int? Aantal { get; set; }
}
