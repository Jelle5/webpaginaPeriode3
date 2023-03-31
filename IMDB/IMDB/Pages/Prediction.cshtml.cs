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
    public double PXLpercent { get; set; }
    public double PXRpercent { get; set; }

    public void OnGet(double PXLpercent2)
    {

        PXLpercent = Math.Round(PXLpercent2, 1);
        PXRpercent = Math.Round(100 - PXLpercent, 1);

    }

    public IActionResult OnPostChance()
    {
        data = new predictRepo().getPred(Request.Form["Genre"]);
        
        gemiddelde = data.Average();
        verwachting = 7.5;
        stdev = StandardDeviation(data);

        Normal normal = new Normal(gemiddelde, stdev);
        PXLpercent = normal.CumulativeDistribution(verwachting) * 100;

        return RedirectToPage(new { PXLpercent2 = PXLpercent} );
    }
    
    static double StandardDeviation(IEnumerable<double> values)
    {
        double avg = values.Average();
        return Math.Sqrt(values.Average(v=>Math.Pow(v-avg,2)));
    }
}
