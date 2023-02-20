using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Pages.Database.model;
using WebApplication2.Pages.Database.repository;

namespace WebApplication2.Pages;

public class Test : PageModel
{
    
    public IEnumerable<Title> title { get; set; }

    public IEnumerable<individual> individuals {get; set;}

    public string Tconsts;
    public string tname;
    public void OnGet(string tconst)
    {
        Tconsts = tconst;
        title = new TitleRepo().getTitle(tconst);
        individuals = new individualRepo().getByTconst(tconst);
        foreach (var titles in title)
        {
            tname = titles.primary;
        }
    }
}