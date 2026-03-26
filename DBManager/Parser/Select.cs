using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DbManager 
{
    // Maialen
    public class Select: MiniSqlQuery
    {
        public string Table { get; private set; }
        public List<string> Columns { get; private set; }
        public Condition Where { get; private set; }

        public Select(string table, List<string> columns, Condition condition=null)
        {
            //TODO DEADLINE 2: Initialize member variables
            this.Table = table;
            this.Columns = columns ?? new List<string> ();
            this.Where = condition;
        }

        public string Execute(Database database)
        {
            //TODO DEADLINE 3: Run the query and return the table as a string (or the last error in the database)
            Table table = database.TableByName(Table);

            if(table==null)
            {
                return Constants.TableDoesNotExistError;
            }

            foreach(string column in Columns)
            {
                if(column != "*" && table.ColumnByName(column)==null)
                {
                    return Constants.ColumnDoesNotExistError;
                }
            }
            if(Where!=null)
            {
                if (string.IsNullOrEmpty(Where.ColumnName) || table.ColumnByName(Where.ColumnName) == null)
                {
                   return Constants.ColumnDoesNotExistError; 
                }
            }
            Table resTable = database.Select(Table, Columns, Where);

            if (resTable == null)
            {
                return database.LastErrorMessage;

            }
            return resTable.ToString();
            
        }
    }
}
