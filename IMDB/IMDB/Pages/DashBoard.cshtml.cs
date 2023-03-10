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
    
    public IEnumerable<Title> types { get; set; }

    public Title title;
    public IActionResult OnGet()
    {
        var cookieOptions = new CookieOptions
        {};
        
        //Vraagt de settings cookie hier op.
         
         string settingsStr = Request.Cookies["settings"];
         settings = new Settings();
         
         if (settingsStr == null)
         {
             string json;
             json = JsonConvert.SerializeObject(settings);
             Response.Cookies.Append("settings", json.ToString(), new CookieOptions()
             {
                 Expires = DateTimeOffset.Now.AddDays(30)
             }); 
             titles = new TitleRepo().getAll();
         }
         else
         {
             settings = JsonConvert.DeserializeObject<Settings>(settingsStr);
             titles = new TitleRepo().settings(settings);
         }

        types = new TitleRepo().getAllType();
        return Page();
    }

    public IActionResult OnPostFilter()
    {
        //Update de settings cookie hier.
        string json;
        json = JsonConvert.SerializeObject(settings);
        
        
        return RedirectToPage();
    }

    public IActionResult OnPostClear()
    {
        settings = new Settings();
        string json;
        json = JsonConvert.SerializeObject(settings);
        
        //Response.Cookies.Delete("settings");
        Response.Cookies.Append("settings", json.ToString());

        return RedirectToPage();
    }
}