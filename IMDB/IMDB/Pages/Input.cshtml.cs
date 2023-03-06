using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Pages.Database.model;

namespace WebApplication2.Pages;

public class Input : PageModel
{
    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPostVraag([FromForm] string vraag)
    {
        var list = GetWords(vraag);
        var final = match(list);
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

    public List<Tuple<string, string>> match(List<string> input)
    {
        lists control = new lists();

        
        
        var intTitle = control.Title.Where(t => input.Contains(t.Item1));
        return intTitle.ToList();
    }
}