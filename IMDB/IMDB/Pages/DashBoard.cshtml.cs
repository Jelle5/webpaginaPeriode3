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

    public string[] countdata = new string[10];
    public string[] averagedata = new string[10];

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
        
        getData();

        return Page();
    }

    public IActionResult OnPostFilter()
    {
        //Update de settings cookie hier.
        string json;
        json = JsonConvert.SerializeObject(settings);
        Response.Cookies.Append("settings", json.ToString());
        
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

    public void getData()
    {
        for (int i = 0; i < 10; i++)
        {
            List<int> count = new List<int>(); // move initialization outside loop
            List<double> average = new List<double>();
            
            foreach (var title in titles)
            {
                int rounded = (int)Math.Round(title.averagerating);
                if (rounded == i)
                {
                    count.Add(rounded); // append to count list
                    average.Add(title.averagerating);
                }
            }

            int total = count.Count;
            string totalstring = total.ToString();
            countdata[i] = totalstring;  // add count for current value of i
            //averagedata[i] = average.Average().ToString();
        }
    }
}