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
    public DataTable result = new DataTable();
    private InputList inputList = InputList.Instance;
    
    

    public List<List<Tuple<string, string>>> feedback = new List<List<Tuple<string, string>>>();
    public IActionResult OnGet()
    {
        if(inputList.GetFeedback())
        {
            feedback =  inputList.GetList();
        }
        else
        {
            if(inputList.GetList().Count > 0 )
                result = ultimatelist.search( inputList.GetList());
        }
        
        return Page();
    }

   

    public IActionResult OnPostQuestion([FromForm] string question)
    {
        question = question.ToLower();
        
        var list = GetWords(question);
        
        inputList.match(list);
        
        inputList.setBoolFeedback(true);
        
        return RedirectToPage();
    }
    public IActionResult OnPostAnswer([FromForm] string answer)
    {
        inputList.setBoolFeedback(false);
        
        List<Tuple<string, string>> where = new List<Tuple<string, string>>();
        foreach (var ints in inputList.GetList()[11])
        {
            where.Add(Tuple.Create(ints.Item1, answer));
        }

        inputList.ChangeAnswerList(where);
        
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
}
