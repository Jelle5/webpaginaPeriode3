namespace WebApplication2.Pages.Database.model;

public class lists
{
    public List<Tuple<string, string>> Title = new List<Tuple<string, string>>
    {
        new Tuple<string, string>( "categorie", "type"),
        new Tuple<string, string>( "titel", "primarytitle"),
        new Tuple<string, string>( "titels", "primarytitle"),
        new Tuple<string, string>( "films", "primarytitle"),
        new Tuple<string, string>( "volwassenen", "isadult"),
        new Tuple<string, string>( "volwassen", "isadult"),
        new Tuple<string, string>( "start", "startyear"),
        new Tuple<string, string>( "eind", "endyear"),
        new Tuple<string, string>( "duur", "runtimemin"),
        new Tuple<string, string>( "waardering", "averagerating"),
        new Tuple<string, string>( "cijfer", "averagerating"),
        new Tuple<string, string>( "rating", "averagerating"),
        new Tuple<string, string>( "stemmen", "numvotes"),
        new Tuple<string, string>( "seizoensnummer", "seasonnr"),
        new Tuple<string, string>( "afleveringsnummer", "episodenr"),
        new Tuple<string, string>( "serie", "parent"),
        new Tuple<string, string>( "duurst", "budget"),
        new Tuple<string, string>( "budget", "budget"),
        new Tuple<string, string>( "opbrengst", "gross_domestic"),
        new Tuple<string, string>( "opbrengst", "gross_worldwide"),
        new Tuple<string, string>( "opbrengst", "opening_weekend"),
        new Tuple<string, string>( "opgebracht", "gross_worldwide"),
        new Tuple<string, string>( "leeftijdsaanduiding", "certificate"),
        new Tuple<string, string>( "herkomst", "origin"),
        new Tuple<string, string>( "land", "filming_country"),
        new Tuple<string, string>( "nederland", "filming_country"),
        new Tuple<string, string>( "nederlands", "filming_country"),
        new Tuple<string, string>( "nederlandse", "filming_country"),
        new Tuple<string, string>( "aspect", "aspect_ratio"),
        new Tuple<string, string>( "ratio", "aspect_ratio")
    };

    public List<Tuple<string, string>> individual = new List<Tuple<string, string>>
    {
        new Tuple<string, string>( "naam", "primaryindividual"),
        new Tuple<string, string>( "namen", "primaryindividual"),
        new Tuple<string, string>( "actrice", "primaryindividual"),
        new Tuple<string, string>( "actrices", "primaryindividual"),
        new Tuple<string, string>( "acteur", "primaryindividual"),
        new Tuple<string, string>( "acteurs", "primaryindividual"),
        new Tuple<string, string>( "voornaam", "primaryindividual"),
        new Tuple<string, string>( "achternaam", "primaryindividual"),
        new Tuple<string, string>( "geboorte", "birthyear"),
        new Tuple<string, string>( "geboren", "birthyear"),
        new Tuple<string, string>( "sterf", "deathyear"),
        new Tuple<string, string>( "sterfjaar", "deathyear"),
        new Tuple<string, string>( "overleden", "deathyear"),
        new Tuple<string, string>( "dood", "deathyear")
        
    };

    public List<Tuple<string, string>> genre = new List<Tuple<string, string>>
    {
        new Tuple<string, string>( "genre", "genre"),
        new Tuple<string, string>( "genres", "genre")
    };

    public List<Tuple<string, string>> profession = new List<Tuple<string, string>>
    {
        new Tuple<string, string>( "rol", "profession"),
        new Tuple<string, string>( "bijdrage", "profession"),
        new Tuple<string, string>( "functie", "profession"),
    };
    
    public List<Tuple<string, string>> ratings = new List<Tuple<string, string>>
    {
        //new Tuple<string, string>( "stemmen", "*"),
        new Tuple<string, string>( "waardering", "*")
    };
    
    public List<Tuple<string, string>> principals = new List<Tuple<string, string>>
    {
        new Tuple<string, string>( "categorie", "category"),
        new Tuple<string, string>( "functie", "category"),
        new Tuple<string, string>( "baan", "job"),
        new Tuple<string, string>( "character", "character"),
       //new Tuple<string, string>( "naam", "characters")
    };

    public List<Tuple<string, string>> company = new List<Tuple<string, string>>
    {
        new Tuple<string, string>( "bedrijf", "company"),
        new Tuple<string, string>( "bedrijfsnaam", "company")
    };
    
    public List<Tuple<string, string>> color = new List<Tuple<string, string>>
    {
        new Tuple<string, string>( "kleur", "name"),
        new Tuple<string, string>( "gekleurd", "name")
    };
    
    public List<Tuple<string, string>> site = new List<Tuple<string, string>>
    {
        new Tuple<string, string>( "webpagina", "site"),
        new Tuple<string, string>( "site", "site"),
        new Tuple<string, string>( "website", "site"),
        new Tuple<string, string>( "link", "link")
    };
    
    public List<Tuple<string, string>> aka = new List<Tuple<string, string>>
    {
        //new Tuple<string, string>( "titel", "title"),
        //new Tuple<string, string>( "titels", "title"),
        new Tuple<string, string>( "regio", "region"),
        new Tuple<string, string>( "taal", "language"),
        new Tuple<string, string>( "hoofdtitel", "isoriginal"),
        //new Tuple<string, string>( "titel", "isoriginal")
    };

    public List<Tuple<string, string>> filter = new List<Tuple<string, string>>
    {
        
        new Tuple<string, string>("laag", "<"),
        new Tuple<string, string>("laagste", "<"),
        new Tuple<string, string>("hoger", ">"),
        new Tuple<string, string>("hogere", ">"),
        new Tuple<string, string>("groter", ">"),
        new Tuple<string, string>("grotere", ">"),
        new Tuple<string, string>("kleiner", "<"),
        new Tuple<string, string>("kleinere", "<"),
        new Tuple<string, string>("minder", "<"),
        new Tuple<string, string>("gelijk", "="),
        new Tuple<string, string>("gelijke", "="),
        new Tuple<string, string>("gelijken", "=")
    };



}