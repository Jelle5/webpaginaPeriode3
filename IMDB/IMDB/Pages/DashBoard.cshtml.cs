using System.Net;
using System.Reflection.Metadata;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebApplication2.Pages.Database.model;
using WebApplication2.Pages.Database.repository;


namespace WebApplication2.Pages;

public class DashBoard : PageModel
{
    [BindProperty] public Settings settings { get; set; }
    public IEnumerable<Title> titles { get; set; }

    public Title title;
    public IActionResult OnGet(bool joe)
    {
    
        //Vraagt de settings cookie hier op.
        settings = new Settings();
        string settingsStr = Request.Cookies["settings"];
        
        if (settingsStr == null)
        {
            Response.Cookies.Append("settings", settings.ToString(), new CookieOptions()
            {
                Expires = DateTimeOffset.Now.AddDays(30)
            }); 
            titles = new TitleRepo().getAll();
        }

        else if(joe)
        {
            settings = JsonConvert.DeserializeObject<Settings>(settingsStr);
            titles = new TitleRepo().settings(settings);
        }

        else
        {
            titles = new TitleRepo().getAll();
        }

        return Page();
    }

    public IActionResult OnPostFilter()
    {
        
        
        //Update de settings cookie hier.
        string json;
        json = JsonConvert.SerializeObject(settings);
        Response.Cookies.Append("settings", json.ToString());

        bool joe;
        return RedirectToPage("/DashBoard", new { joe = true });
    }

}