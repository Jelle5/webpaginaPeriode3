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

    public IActionResult OnPostFilter([FromForm] string type, [FromForm] string isAdult, [FromForm] string is1, [FromForm] string startyear, 
        [FromForm] string is2, [FromForm] string endyear, [FromForm] string is3, [FromForm] string runtimeMinutes, 
        [FromForm] string is4, [FromForm] string averageRating, [FromForm] string is5, [FromForm] string numVotes, 
        [FromForm] string is6, [FromForm] string seasonnr, [FromForm] string episodenr)
    {
        
        
        return RedirectToPage("/DashBoard");
    }

}