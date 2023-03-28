
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication2.Pages;

public class Genre : PageModel
{
    public string[] x { get; set; }
    public double[] y { get; set; }
    public int[] size { get; set; }
    
    public void OnGet()
    {
        // Load your data here
        string json = System.IO.File.ReadAllText("GraphData/avgRatingCountGenre.json");
        var data = JsonSerializer.Deserialize<List<MyData>>(json);

        // Convert to arrays
        x = data.Select(x => x.x).ToArray();
        y = data.Select(x => Math.Round(x.y, 1)).ToArray();
        size = data.Select(x => x.r / 25000).ToArray();
    }
}

public class MyData
{
    public string x { get; set; }
    public double y { get; set; }
    public int r { get; set; }
}