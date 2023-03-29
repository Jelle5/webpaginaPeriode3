using WebApplication2.Pages.Database.model;

namespace WebApplication2;

public class InputList
{
    private static InputList instance = null;
    
    private List<List<Tuple<string, string>>> InputMatch = new List<List<Tuple<string, string>>>();
    private bool feedbackNeeded = false;

    private InputList()
    {
        
    }

    public static InputList Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new InputList();
            }
            return instance;

        }
    }
    
    public void match(List<string> input)
    {
        InputMatch = new List<List<Tuple<string, string>>>();

        lists control = new lists();

        InputMatch.Add(control.Title.Where(t => input.Contains(t.Item1)).ToList());
        
        InputMatch.Add(control.individual.Where(t => input.Contains(t.Item1)).ToList());
        
        InputMatch.Add(control.genre.Where(t => input.Contains(t.Item1)).ToList());
        
        InputMatch.Add(control.profession.Where(t => input.Contains(t.Item1)).ToList());
        
        InputMatch.Add(control.ratings.Where(t => input.Contains(t.Item1)).ToList());
        
        InputMatch.Add(control.principals.Where(t => input.Contains(t.Item1)).ToList());
        
        InputMatch.Add(control.company.Where(t => input.Contains(t.Item1)).ToList());
        
        InputMatch.Add(control.color.Where(t => input.Contains(t.Item1)).ToList());
        
        InputMatch.Add(control.site.Where(t => input.Contains(t.Item1)).ToList());
        
        InputMatch.Add(control.aka.Where(t => input.Contains(t.Item1)).ToList());
        
        List<Tuple<string, string>> filters =  control.filter.Where(t => input.Contains(t.Item1)).ToList();

        List<Tuple<string, string>> ints = new List<Tuple<string, string>>();
        
        if (filters.Count != 0)
        {
            foreach (var inputs in input)
            {
                try
                { 
                    float.Parse(inputs);
                    ints.Add(Tuple.Create(inputs, ""));
                }
                catch (Exception e)
                {

                }
            }
        }
        InputMatch.Add(filters);
        InputMatch.Add(ints);
        
    }

    public List<List<Tuple<string, string>>> GetList()
    {
        return InputMatch;
    }
    
    public void setBoolFeedback(bool state)
    {
        if (InputMatch[11].Count > 0)
        {
            feedbackNeeded = state;
        }
    }

    public bool GetFeedback()
    {
        return feedbackNeeded;
    }
    
    public List<List<Tuple<string, string>>> ChangeAnswerList(List<Tuple<string, string>> change)
    {
        InputMatch.RemoveAt(11);
        InputMatch.Add(change);
        return InputMatch;
    }
    
    
}
