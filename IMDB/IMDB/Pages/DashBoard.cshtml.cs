using System.Net;
using System.Reflection.Metadata;
using System.Security;
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

    public List<object> dataOpeningDomestic = new List<object>();
    public List<object> dataOpeningWorldwide = new List<object>();
    public List<object> dataOpeningRating = new List<object>();

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
        
        getData(titles);

        return Page();
    }

    public IActionResult OnPostFilter()
    {
        //Update the settings cookie here.
        string json;
        json = JsonConvert.SerializeObject(settings);
        Response.Cookies.Append("settings", json.ToString(), new CookieOptions()
        {
            Expires = DateTimeOffset.Now.AddDays(30)
        });
    
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

    // public void getData(IEnumerable<Title> titles)
    // {
    //     string filePath = "csvData/DashboardTable.csv";
    //     using (StreamWriter writer = new StreamWriter(filePath))
    //     {
    //         var headers = titles.First().GetType().GetProperties().Where(p => p.Name != "Principals").Select(p => p.Name);
    //         writer.WriteLine(string.Join(";", headers));
    //
    //         foreach (var row in titles)
    //         {
    //             var values = row.GetType().GetProperties().Select(p => p.GetValue(row, null));
    //             var valueStrings = values.Select(v => v == null ? "" : v.ToString());
    //             writer.WriteLine(string.Join(";", valueStrings));
    //         }
    //     }
    // }

    public void getData(IEnumerable<Title> titles)
    {
        foreach (var title in titles)
        {
            dataOpeningDomestic.Add(new { x = title.opening_weekend, y = title.gross_domestic, name = title.primary});
            dataOpeningWorldwide.Add(new { x = title.opening_weekend, y = title.gross_worldwide, name = title.primary});
            dataOpeningRating.Add(new { x = title.opening_weekend, y = title.averagerating, name = title.primary});
        }
    }
}