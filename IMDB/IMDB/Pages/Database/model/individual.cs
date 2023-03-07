namespace WebApplication2.Pages.Database.model;

public class individual
{
    public string nconst { get; set; }
    
    public string primary { get; set; }
    
    public int? birthyear { get; set; }
    
    public int? deathyear { get; set; }
    
    public principals Principals { get; set; }
}