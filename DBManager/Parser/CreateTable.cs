using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;

namespace DbManager
{
    
    // Maialen
    public class CreateTable : MiniSqlQuery
    {
        public string Table { get; private set; }
        public List<ColumnDefinition> ColumnsParameters { get; private set; } = new List<ColumnDefinition>();

        public CreateTable(string table, List<ColumnDefinition> columns)
        {
            //TODO DEADLINE 2: Initialize member variables
            this.Table= table;
            this.ColumnsParameters=columns;
            
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 3: Run the query and return the appropriate message
            //CreateTableSuccess or the last error in the database
            
            if(database== null) 
            return Constants.Error;
            Table tabla = database.TableByName(Table);
           
            if(tabla!=null) 
            return Constants.TableAlreadyExistsError;
            
            if(ColumnsParameters==null||ColumnsParameters.Count==0) 
            return Constants.DatabaseCreatedWithoutColumnsError;
            
            if(database.CreateTable(Table,ColumnsParameters))
            return Constants.CreateTableSuccess;
            
            return Constants.Error;
        }

    }
}
