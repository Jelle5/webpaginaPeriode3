using System.Collections;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Pages.Database.model;
using WebApplication2.Pages.Database.repository;

namespace WebApplication2.Pages;

public class PersonInfo : PageModel
{
    public IEnumerable<profession> professions { get; set; }
    public IEnumerable<individual> individuals {get; set;}

    public IEnumerable<Title> Titles { get; set; }

    public string Nconst;
    public string nname;
    
    public void OnGet(string nconst)
    {
        Nconst = nconst;
        individuals = new individualRepo().getByNconst(nconst);
        professions = new professionRepo().getByNconst(nconst);
        Titles = new TitleRepo().getByNconst(nconst);
        foreach (var individual in individuals)
        {
            nname = individual.primary;
        }
    }
}