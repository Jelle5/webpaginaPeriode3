using Microsoft.AspNetCore.Mvc.RazorPages;
using MathNet.Numerics.LinearRegression;
using System.IO;
using System.Linq;

namespace WebApplication2.Pages;

public class Prediction : PageModel
{
    public static string relativePath = @"Images/Rpred.png";

    public static string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

    public string fullPath = Path.Combine(baseDirectory, relativePath);
    
    

    public void OnGet()
    {
        //PREDICT.Attribute["src"] = fullPath;
    }
}