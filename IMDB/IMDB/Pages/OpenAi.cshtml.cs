using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenAI_API.Models;
using Newtonsoft.Json;
using WebApplication2.Pages.Database.model;
using WebApplication2.Pages.Database.repository;

namespace WebApplication2.Pages;

public class OpenAi : PageModel
{
    public async Task<IActionResult> OnPost(string query)
    {
        try
        {
            var api = new OpenAI_API.OpenAIAPI("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");
            var response = await api.Completions.CreateCompletionAsync(
                "### Postgres SQL tables, with their properties:\n#\n# Employee(id, name, department_id)\n# Department(id, name, address)\n# Salary_Payments(id, employee_id, amount, date)\n#\n### A query to list the names of the departments which employed more than 10 employees in the last 3 months\nSELECT",
                model: Model.DavinciCode, 
                temperature: 0, 
                max_tokens: 150, 
                top_p: 1.0, 
                frequencyPenalty: 0.0, 
                presencePenalty: 0.0
                );
            var result = response.ToString();
            Response.Cookies.Append("openairesult", result, new CookieOptions()
            {
                Expires = DateTimeOffset.Now.AddDays(30)
            });
            return Page();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}