namespace WebApplication2.Pages.Database.model;

public class Title
{
    public string tconst { get; set; }
    
    public string type { get; set; }
    
    public string primary { get; set; }
    
    public bool isadult { get; set; }
    
    public int startyear { get; set; }
    
    public int endyear { get; set; }
    
    public int runtimemin { get; set; }

    public double averagerating { get; set; }
     
    public int numvotes { get; set; }
    
    public int seasonnr { get; set; }
    
    public int episodenr { get; set; }
    
    public string parent { get; set; }
    
    public int budget { get; set; }
    
    public int gross_domestic { get; set; }
    
    public int gross_worldwide { get; set; }
    
    public int opening_weekend { get; set; }
    
    public string certificate { get; set; }
    
    public string origin { get; set; }
    
    public string filming_country { get; set; }
    
    public string aspect_ratio { get; set; }
    public principals Principals { get; set; }
}