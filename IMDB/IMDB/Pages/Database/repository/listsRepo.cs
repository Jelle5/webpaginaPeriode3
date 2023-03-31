using System.Collections;
using System.Data;
using Dapper;
using WebApplication2.Pages.Database.model;

namespace WebApplication2.Pages.Database.repository;

public class listsRepo
{
    private IDbConnection getConnection()
    {
        return new DbUtils().Connect();
    }

    public DataTable search(List<List<Tuple<string, string>>> ultimate)
    {
        var sqlSelect = new List<string>();
        var where = new List<string>();
        var queryParams = new DynamicParameters();
        var sqlFromTable = new List<string>();
        int count = 0;

        List<Tuple<string, string, string>> table = new List<Tuple<string, string, string>>
        {
            new ("title", "", "tconst"),
            new ("individual", "principals", "nconst"),
            new ("genre", "genres", "gconst"),
            new ("profession", "professions", "pconst"),
            new ("ratings", "", ""),
            new ("principals", "title", "tconst"),
            new ("company", "companies", "coconst"),
            new ("color", "colors", "cconst"),
            new ("site", "sites", "sconst"),
            new ("aka", "akas", "aconst")
        };
        
        foreach (var lijst in ultimate)
        {
            if (lijst.Count != 0)
            {
                if (count != 0)
                {
                    

                    if (count == 5)
                    {
                        if (ultimate[1].Count == 0)
                        {
                            sqlFromTable.Add(" INNER JOIN " + table[count].Item1 + " on " + table[count].Item1 + ".nconst = "+ table[count].Item2+ ".nconst \n");
                        }
                    }
                    else if (count == 10)
                    {
                        if (lijst.Count != 0)
                        {
                            foreach (var statement in lijst)
                            {
                                foreach (var columns in ultimate[11])
                                {
                                    where.Add(columns.Item2 + " " + statement.Item2 + " " +
                                              columns.Item1.Replace(",", "."));
                                }

                            }

                        }
                    }
                    else if(count != 11)
                    {
                        sqlFromTable.Add(" INNER JOIN " + table[count].Item2 + " on " + table[count].Item2 + ".tconst = title.tconst \n"  );
                        sqlFromTable.Add(" INNER JOIN " + table[count].Item1 + " on " + table[count].Item1 + "." + table[count].Item3 + " = "+ table[count].Item2+ "." + table[count].Item3 + "\n");
                    }

                   
                }

                if (lijst != ultimate[4] && lijst != ultimate[10] && lijst != ultimate[11])
                {
                    foreach (var lijstje in lijst)
                    {
                        sqlSelect.Add(table[count].Item1 + "." + lijstje.Item2);
                    }
                }
            }

            count++;
        }


        string sql = @"SELECT ";
        
        foreach (var select in sqlSelect)
        {
            if (select == sqlSelect.Last())
            {
                sql += select;
            }
            else
            {
                sql += select + ", ";
            }
        }

        sql += " FROM title";
        foreach (var inner in sqlFromTable)
        {
            sql += inner;
        }

        if (where.Count > 0)
        {
            sql += " WHERE ";
        }
        
            foreach (var whereclause in where)
            {
                
                if (whereclause == where[0])
                {
                    sql += whereclause + " ";
                }
                else
                {
                    sql += "AND " + whereclause + " ";
                }
                
            }
        
        sql += " LIMIT 100";
        using var connection = getConnection();
        IEnumerable<dynamic>? dapperList;
        try
        {
            dapperList = connection.Query(sql, queryParams).ToList();
        }
        catch (Exception e)
        {
            dapperList = connection.Query("Select * from title Limit 100");
        }

        return tableBuilder(dapperList);
    }

    private DataTable tableBuilder(IEnumerable<dynamic>? dapperList)
    {
        DataTable dt = new DataTable();
        
        // voeg kolomen toe op basis van de properties van de eerste rij
        dynamic firstRow = dapperList.FirstOrDefault();
        if (firstRow != null)
        {
            foreach (var property in firstRow)
            {
                dt.Columns.Add(property.Key, property.Value?.GetType() ?? typeof(object));
            }
        }

        // voeg rows toe
        foreach (dynamic row in dapperList)
        {
            DataRow dr = dt.NewRow();

            foreach (var property in row)
            {
                dr[property.Key] = property.Value ?? DBNull.Value;
            }

            dt.Rows.Add(dr);
        }
        
        return dt;
    }
}