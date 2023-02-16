using System.Data;

using Microsoft.Extensions.Configuration;
using Npgsql;

namespace WebApplication2.Pages.Database
{
    public class DbUtils
    {   
        public IDbConnection Connect()
        {
            var connString = "Host=82.73.46.252:5432;Username=Website;Password=IMDB-Website-Groep10;Database=imdb";

            return new NpgsqlConnection(connString);
            
        }
    }
}