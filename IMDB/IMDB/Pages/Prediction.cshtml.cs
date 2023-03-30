using System.Collections;
using MathNet.Numerics;
using MathNet.Numerics.LinearRegression;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Pages.Database.model;
using WebApplication2.Pages.Database.repository;


namespace WebApplication2.Pages;

public class Prediction : PageModel
{
    public IEnumerable<Title> titles { get; set; }
    public IEnumerable<Title> selection { get; set; }

    public IEnumerable<double> data { get; set; }
    
    [BindProperty]
    public string Genre { get; set; }
    double gemiddelde { get; set; }
    double verwachting { get; set; }
    double stdev { get; set; }
    double Z { get; set; }
    double PXLpercent { get; set; }
    double PXRpercent { get; set; }

    public void OnGet()
    {
        titles = new TitleRepo().getAllMovies();
        
        var random = new Random();
        selection = titles.OrderBy(m => random.Next()).Take(100).ToList();
    }

    public IActionResult OnPostChance()
    {
        data = new predictRepo().getPred(Request.Form["Genre"]);
        
        gemiddelde = data.Average();
        verwachting = 7.5;
        stdev = StandardDeviation(data);
        Z = (verwachting - gemiddelde) / stdev;

        var normal = new Normal(gemiddelde, stdev);
        PXLpercent = normal.InverseCumulativeDistribution(Z);;
        PXRpercent = 100 - PXLpercent;
        return RedirectToPage();
    }
    
    static double StandardDeviation(IEnumerable<double> values)
    {
        double avg = values.Average();
        return Math.Sqrt(values.Average(v=>Math.Pow(v-avg,2)));
    }
}
