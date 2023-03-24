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
        
        toCsv(titles);
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

    public void toCsv(IEnumerable<Title> titles)
    {
        string filePath = "csvData/DashboardTable.csv";
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            var headers = titles.First().GetType().GetProperties().Where(p => p.Name != "Principals").Select(p => p.Name);
            writer.WriteLine(string.Join(";", headers));
    
            foreach (var row in titles)
            {
                var values = row.GetType().GetProperties().Select(p => p.GetValue(row, null));
                var valueStrings = values.Select(v => v == null ? "" : v.ToString());
                writer.WriteLine(string.Join(";", valueStrings));
            }
        }
    }
    
    public List<object> OpeningDomestic = new List<object>();
    public List<object> OpeningWorldwide = new List<object>();
    public List<object> OpeningRating = new List<object>();
    public List<string> Countries = new List<string>();
    public IEnumerable<string> CountriesLabels = new List<string>();
    public List<string> CountriesCount = new List<string>();
    
    public void getData(IEnumerable<Title> titles)
    {
        foreach (var title in titles)
        {
            OpeningDomestic.Add(new { x = title.opening_weekend, y = title.gross_domestic});
            OpeningWorldwide.Add(new { x = title.opening_weekend, y = title.gross_worldwide});
            OpeningRating.Add(new { x = title.opening_weekend, y = title.averagerating});
            Countries.Add(title.origin);
        }

        CountriesLabels = Countries.Distinct();

        foreach (var country in Countries)
        {
            
        }
    }
}