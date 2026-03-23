using DbManager.Parser;
using System.Collections.Generic;

namespace DbManager
{
    public class Update: MiniSqlQuery
    {
        // Endika
        public string Table { get; private set; }
        public List<SetValue> Columns { get; private set; }
        public Condition Where { get; private set; }

        public Update(string table, List<SetValue> columnNames, Condition where)
        {
            //TODO DEADLINE 2: Initialize member variables
            this.Table = table;
            this.Columns = columnNames;
            this.Where = where;
        }

        public string Execute(Database database)
        {
            //TODO DEADLINE 3: Run the query and return the appropriate message
            //UpdateSuccess or the last error in the database
            if (database == null)
            { 
                return Constants.Error;
            }
            Table table = database.TableByName(Table);
            if (table == null)
            {
                return Constants.TableDoesNotExistError;
            }
            foreach(SetValue setValue in Columns)
            {
                bool exists=false;
                for(int j=0;j<table.NumColumns();j++)
                {
                    if (table.GetColumn(j).Name==setValue.ColumnName)
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                {
                    return database.LastErrorMessage;
                }
            }
            for (int k = 0; k < table.NumRows(); k++)
            {
                Row row = table.GetRow(k);
                if (Where == null || row.IsTrue(Where))
                {
                    foreach (SetValue setValue in Columns)
                    {
                        for (int l = 0; l < table.NumColumns(); l++)
                        {
                            if (table.GetColumn(l).Name == setValue.ColumnName)
                            {
                                row.Values[l] = setValue.Value;
                                break;
                            }
                        }
                    }
                }
            }
            return Constants.UpdateSuccess;
        } 
    }
}