using MathNet.Numerics;
using MathNet.Numerics.LinearRegression;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Pages.Database.model;
using WebApplication2.Pages.Database.repository;

namespace WebApplication2.Pages;

public class Prediction : PageModel
{
    public IEnumerable<Title> titles { get; set; }
    public IEnumerable<Title> selection { get; set; }

    public void OnGet()
    {
        titles = new TitleRepo().getAllMovies();

        var random = new Random();
        selection = titles.OrderBy(m => random.Next()).Take(100).ToList();


    }
}
