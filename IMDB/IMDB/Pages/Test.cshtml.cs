using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Pages.Database.model;
using WebApplication2.Pages.Database.repository;

namespace WebApplication2.Pages;

public class Test : PageModel
{
    
    public IEnumerable<Title> titles { get; set; }

    public IEnumerable<individual> individuals {get; set;}

    public string Tconsts;

    public void OnGet(string tconst)
    {
        Tconsts = tconst;
        individuals = new individualRepo().getByTconst(tconst);
    }
}