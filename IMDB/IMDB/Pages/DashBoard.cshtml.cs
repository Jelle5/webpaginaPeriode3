using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Pages.Database.model;
using WebApplication2.Pages.Database.repository;


namespace WebApplication2.Pages;

public class DashBoard : PageModel
{

    public IEnumerable<Title> titles { get; set; }

    public Title title;
    public void OnGet()
    {
        titles = new TitleRepo().getAll();
    }

    public IActionResult OnPostFilter([FromForm] string type, [FromForm] string isAdult, [FromForm] string startyear, [FromForm] string endyear, [FromForm] string runtimeMinutes, [FromForm] string averageRating, [FromForm] string numVotes, [FromForm] string seasonnr, [FromForm] string episodenr)
    {
        
        
        return RedirectToPage("/TitleInfo");
    }

}