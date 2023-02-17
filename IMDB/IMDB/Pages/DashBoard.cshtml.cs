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
}