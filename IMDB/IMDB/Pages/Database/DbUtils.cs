using System.Data;

using Microsoft.Extensions.Configuration;
using Npgsql;

namespace WebApplication2.Pages.Database
{
    public class DbUtils
    {   
        public IDbConnection Connect()
        {
            var connString = "Host=84.29.143.210:5432;Username=Website;Password=IMDB-Website-Groep10;Database=imdb";

            return new NpgsqlConnection(connString);
            
        }
    }
}