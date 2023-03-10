using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Pages.Database.model;

public class Settings
{
     public string type { get; set; } = "movie";
     public string isAdult { get; set; } = "";
     [StringLength(1)] public string isstart { get; set; }   = "";
     public int startyear { get; set; }  = 0;
     [StringLength(1)] public string isend { get; set; }   = "";
     public int endyear { get; set; }   = 0;
     [StringLength(1)] public string isruntime { get; set; }   = "";
     public int runtimeMinutes { get; set; }  = 0;
     [StringLength(1)] public string isavg { get; set; } = "";
     public double averagerating { get; set; }   = 0;
     [StringLength(1)] public string isnum { get; set; }   = ">";
     public int numVotes { get; set; }  = 5000;
     [StringLength(1)] public string isseason { get; set; }  = "";
     public int seasonnr { get; set; } = 0;
     public int episodenr { get; set; } = 0;
     public int limit { get; set; } = 0;
     
}