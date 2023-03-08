using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Pages.Database.model;
using WebApplication2.Pages.Database.repository;

namespace WebApplication2.Pages;

public class Input : PageModel
{
    private listsRepo ultimatelist = new listsRepo();
    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPostVraag([FromForm] string vraag)
    {
        
        vraag = vraag.ToLower();
        var list = GetWords(vraag);
        var final = match(list);
        ultimatelist.search(final);
        return RedirectToPage();
    }
    
    static List<string> GetWords(string input)
    {
        MatchCollection matches = Regex.Matches(input, @"\b[\w']*\b");

        var words = from m in matches.Cast<Match>()
            where !string.IsNullOrEmpty(m.Value)
            select TrimSuffix(m.Value);

        return words.ToList();
    }

    static string TrimSuffix(string word)
    {
        int apostropheLocation = word.IndexOf('\'');
        if (apostropheLocation != -1)
        {
            word = word.Substring(0, apostropheLocation);
        }

        return word;
    }

    public List<List<Tuple<string, string>>> match(List<string> input)
    {
        List<List<Tuple<string, string>>> ultimate = new List<List<Tuple<string, string>>>();
        
        lists control = new lists();

        ultimate.Add(control.Title.Where(t => input.Contains(t.Item1)).ToList());
        
        ultimate.Add(control.individual.Where(t => input.Contains(t.Item1)).ToList());
        
        ultimate.Add(control.genre.Where(t => input.Contains(t.Item1)).ToList());
        
        ultimate.Add(control.profession.Where(t => input.Contains(t.Item1)).ToList());
        
        ultimate.Add(control.ratings.Where(t => input.Contains(t.Item1)).ToList());
        
        ultimate.Add(control.principals.Where(t => input.Contains(t.Item1)).ToList());
        
        ultimate.Add(control.company.Where(t => input.Contains(t.Item1)).ToList());
        
        ultimate.Add(control.color.Where(t => input.Contains(t.Item1)).ToList());
        
        ultimate.Add(control.site.Where(t => input.Contains(t.Item1)).ToList());
        
        ultimate.Add(control.aka.Where(t => input.Contains(t.Item1)).ToList());

        return ultimate;
    }
}