using Microsoft.AspNetCore.Mvc.RazorPages;
using MathNet.Numerics.LinearRegression;
using System.IO;
using System.Linq;

namespace WebApplication2.Pages;

public class Prediction : PageModel
{
    public void OnGet()
    {
        var data = model();

        double[] y = { };
        double[][] x = { };

        var regression = MultipleRegression.NormalEquations(x, y);
        

    }

    public IEnumerable<IEnumerable<string>> model()
    {
        var lines = System.IO.File.ReadAllLines("csvData/DashboardTable.csv");

        var header = lines[0].Split(';');

        var columnIndex = Array.IndexOf(header, "column_name");

        var columnIndices = header.Select((name, index) => new { Name = name, Index = index })
            .Where(column => new[] { "tconst", "type", "isadult", "startyear", "endyear", "runtimemin", "averagerating", "numvotes", "seasonnr", "episodenr", "parent", "budget", "gross_domestic", "gross_worldwide", "opening_weekend", "origin", "filming_country", "aspect_ratio" }.Contains(column.Name))
            .Select(column => column.Index);

        var columnData = lines.Skip(1).Select(line => columnIndices.Select(index => line.Split(';')[index]));
        return columnData;
    }
}