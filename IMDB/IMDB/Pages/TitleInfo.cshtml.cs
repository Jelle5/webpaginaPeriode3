using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Pages.Database.model;
using WebApplication2.Pages.Database.repository;

namespace WebApplication2.Pages;

public class Test : PageModel
{
    
    public IEnumerable<Title> title { get; set; }

    public IEnumerable<individual> individuals {get; set;}
    
    public IEnumerable<aka> Akas { get; set; }

    public IEnumerable<company> Companies { get; set; }
    
    public string Tconsts;
    public string tname;
    public string seasonepisode;
    public void OnGet(string tconst)
    {
        Tconsts = tconst;
        title = new TitleRepo().getTitle(tconst);
        individuals = new individualRepo().getByTconst(tconst);
        Akas = new akaRepo().getByTconst(tconst);
        Companies = new companyRepo().getByTconst(tconst);
        foreach (var title in title)
        {
            tname = title.primary;
            seasonepisode = "S" + @title.seasonnr + "." + @title.episodenr;

            if (title.startyear == 0)
                title.startyear = null;
            if (title.endyear == 0)
                title.endyear = null;
        }

        foreach (var individual in individuals)
        {
            if (individual.birthyear == 0)
                individual.birthyear = null;
            if (individual.deathyear == 0)
                individual.deathyear = null;
        }
        
    }
}