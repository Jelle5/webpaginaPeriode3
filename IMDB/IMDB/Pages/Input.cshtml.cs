using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Pages.Database.model;
using WebApplication2.Pages.Database.repository;
using System;
using System.Data;
using Newtonsoft.Json;

namespace WebApplication2.Pages;

public class Input : PageModel
{
    private listsRepo ultimatelist = new listsRepo();
    public DataTable answer = new DataTable();
    

    public List<List<Tuple<string, string>>> feedback = new List<List<Tuple<string, string>>>();
    public IActionResult OnGet()
    {
        
        string ultimate = Request.Cookies["answerlist"];
        if (!string.IsNullOrEmpty(ultimate))
        {
            answer = ultimatelist.search( JsonConvert.DeserializeObject<List<List<Tuple<string, string>>>>(ultimate));
        }
        string Feedback = Request.Cookies["ultimatelist"];
        if (!string.IsNullOrEmpty(Feedback))
        {
            this.feedback =  JsonConvert.DeserializeObject<List<List<Tuple<string, string>>>>(Feedback);
        }

        return Page();
    }

   

    public IActionResult OnPostVraag([FromForm] string vraag)
    {
        Response.Cookies.Delete("answerlist");
        Response.Cookies.Delete("ultimatelist");
        vraag = vraag.ToLower();
        string json;
        var list = GetWords(vraag);
        var final = match(list);
        if (final[11].Count == 0)
        {
             json = JsonConvert.SerializeObject( final);
             
             string ultimatelist = Request.Cookies["answerlist"];
             if (ultimatelist == null)
             {
                 Response.Cookies.Append("answerlist", json, new CookieOptions()
                 {
                     Expires = DateTimeOffset.Now.AddDays(30)
                 });
             }
             else
             {
                 Response.Cookies.Append("answerlist", json);
             }
        }
        else
        {

            json = JsonConvert.SerializeObject(final);
            string ultimatelist = Request.Cookies["ultimatelist"];
            if (ultimatelist == null)
            {
                Response.Cookies.Append("ultimatelist", json, new CookieOptions()
                {
                    Expires = DateTimeOffset.Now.AddDays(30)
                });
            }
            else
            {
                Response.Cookies.Append("ultimatelist", json);
            }
        }

        
        return RedirectToPage();
    }
    public IActionResult OnPostAntwoord([FromForm] string antwoord)
    {
        string ultimate = Request.Cookies["ultimatelist"];
        Response.Cookies.Delete("ultimatelist");
        feedback = JsonConvert.DeserializeObject<List<List<Tuple<string, string>>>>(ultimate);
        List<Tuple<string, string>> where = new List<Tuple<string, string>>();
        foreach (var ints in feedback[11])
        {
            where.Add(Tuple.Create(ints.Item1, antwoord));
        }
        feedback.RemoveAt(11);
        feedback.Add(where);
        string json = JsonConvert.SerializeObject( feedback);
             
        string ultimatelist = Request.Cookies["answerlist"];
        if (ultimatelist == null)
        {
            Response.Cookies.Append("answerlist", json, new CookieOptions()
            {
                Expires = DateTimeOffset.Now.AddDays(30)
            });
        }
        else
        {
            Response.Cookies.Append("answerlist", json);
        }
        
        return RedirectToPage();
    }
    
    static List<string> GetWords(string input)
    {
        MatchCollection matches = Regex.Matches(input, @"\b[\w',.]*\b");

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
        
        List<Tuple<string, string>> filters =  control.filter.Where(t => input.Contains(t.Item1)).ToList();

        List<Tuple<string, string>> ints = new List<Tuple<string, string>>();
        
        if (filters.Count != 0)
        {
            foreach (var inputs in input)
            {
                try
                { 
                    float.Parse(inputs);
                    ints.Add(Tuple.Create(inputs, ""));
                }
                catch (Exception e)
                {

                }
            }
        }
        ultimate.Add(filters);
        ultimate.Add(ints);

        return ultimate;
    }
}
