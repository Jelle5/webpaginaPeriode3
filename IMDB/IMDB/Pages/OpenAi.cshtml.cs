using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpenAI_API;
using Newtonsoft.Json;
using OpenAI_API.Completions;
using WebApplication2.Pages.Database.model;
using WebApplication2.Pages.Database.repository;
using Model = OpenAI_API.Models.Model;

namespace WebApplication2.Pages;

public class OpenAi : PageModel
{
    public string input { get; set; }
    public string SqlQuery { get; set; }
    public string TranslatedQuery { get; set; }
    
    public async Task<IActionResult> OnPostAsync(string query)
    {
        // if (string.IsNullOrEmpty(SqlQuery))
        // {
        //     return Page();
        // }
        
        var prompt = 
            "### Postgres SQL tables, with their properties:" +
            "\n#" +
            "\n# aka(aconst, isoriginal, language, region, title)" +
            "\n# akas(tconst, aconst)" +
            "\n# characters(nconst, tconst, characters)" +
            "\n# color(cconst, name)" +
            "\n# colors(tconst, cconst)" +
            "\n# company(coconst, name)" +
            "\n# companies(tconst, coconst)" +
            "\n# demographics(tconst, all_all_ages_rating, all_all_ages_numvotes, all_under_18_rating, all_under_18_numvotes, all_18_29_rating, all_18_29_numvotes, all_30_44_rating, all_30_44_numvotes, all_45_plus_ratin, all_45_plus_numvotes, male_all_ages_rating, male_all_ages_numvotes, male_under_18_rating, male_under_18_numvotes, male_18_29_rating, male_18_29_numvotes, male_30_44_rating, male_30_44_numvotes, male_45_plus_rating, male_45_plus_numvotes, female_all_ages_rating, female_all_ages_numvotes, female_under_18_rating, female_under_18_numvotes, female_18_29_rating, female_18_29_numvotes, female_30_44_rating, female_30_44_numvotes, female_45_plus_rating, emale_45_plus_numvotes)" +
            "\n# genre(gconst, name)" +
            "\n# genres(tconst, gconst)" +
            "\n# individual(nconst, primary, birthyear, deathyear)" +
            "\n# language(lconst, name)" +
            "\n# languages(tconst, lconst)" +
            "\n# profession(pconst, profession)" +
            "\n# professions(nconst, pconst)" +
            "\n# principals(nconst, tconst, category, job, characters)" +
            "\n# ratings(tconst, mean, median, 10_percentage, 10_numvotes, 9_percentage, 9_numvotes, 8_percentage, 8_numvotes, 7_percentage, 7_numvotes, 6_percentage, 6_numvotes, 5_percentage, 5_numvotes, 4_percentage, 4_numvotes, 3_percentage, 3_numvotes, 2_percentage, 2_numvotes, 1_percentage, 1_numvotes)" +
            "\n# site(sconst, name, link)" +
            "\n# sites(tconst, sconst)" +
            "\n# soundmix(soconst, name)" +
            "\n# soundmixes(tconst, soconst)" +
            "\n# title(tconst, type, primary, isadult, startyear, endyear, runtimemin, averagerating, numvotes, seasonnr, episodenr, parent, budget, gross_domestic, gross_worldwide, opening_weekend, certificate, origin, filming_country, aspect_ratio)" +
            "\n### " + input +
            "\nSELECT " + SqlQuery;

        OpenAIAPI api = new OpenAI_API.OpenAIAPI("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");

        var completions = await api.Completions.CreateCompletionAsync(
        prompt, 
        model: Model.DavinciCode, 
        temperature: 0, 
        max_tokens: 150, 
        top_p: 1.0, 
        frequencyPenalty: 0.0, 
        presencePenalty: 0.0);
        
        TranslatedQuery = completions.ToString().Trim();
        
        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(30)
        };
        Response.Cookies.Append("Query", TranslatedQuery, cookieOptions);
        
        return Page();
    }

}