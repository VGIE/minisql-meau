using DbManager.Parser;
using DbManager.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager
{
    public class Database
    {
        private List<Table> Tables = new List<Table>();
        private string m_username;

        public string LastErrorMessage { get; private set; }

        public Manager SecurityManager { get; private set; }

        //This constructor should only be used from Load (without needing to set a password for the user). It cannot be used from any other class
        private Database() // Maialen
        {
            Tables = new List<Table>();
            LastErrorMessage = string.Empty;
            SecurityManager = new Manager("system");
            
        }

        public Database(string adminUsername, string adminPassword) // Maialen
        {
            //DEADLINE 1.B: Initalize the member variables
            m_username = adminUsername;
            Tables = new List<Table>();
            LastErrorMessage = string.Empty;
            SecurityManager = new Manager(adminUsername);

           
            
        }

        public bool AddTable(Table table) // Aitana
        {
            //DEADLINE 1.B: Add a new table to the database

            if (table == null)
            {
                return false;
            }

            
            if (TableByName(table.Name) != null)
            {
                LastErrorMessage = Constants.TableAlreadyExistsError;
                return false;
            }

            Tables.Add(table);

          
            LastErrorMessage = Constants.CreateTableSuccess;

            return true;

        }

        public Table TableByName(string tableName) // Unai
        {
            //DEADLINE 1.B: Find and return the table with the given name
            foreach (Table table in Tables)
            {
                if (table.Name == tableName)
                {
                    return table;
                }
            }
            return null;
        }

        public bool CreateTable(string tableName, List<ColumnDefinition> ColumnDefinition) // Unai
        {
            //DEADLINE 1.B: Create and new table with the given name and columns. If there is already a table with that name,
            //return false and set LastErrorMessage with the appropriate error (Check Constants.cs)
            //Do the same if no column is provided
            //If everything goes ok, set LastErrorMessage with the appropriate success message (Check Constants.cs)
            if (TableByName(tableName) != null)
            {
                LastErrorMessage = Constants.TableAlreadyExistsError;
                return false;
            }

            if (ColumnDefinition.Count == 0)
            {
                LastErrorMessage = Constants.DatabaseCreatedWithoutColumnsError;
                return false;
            }

            Table newTable = new(tableName, ColumnDefinition);
            Tables.Add(newTable);

            LastErrorMessage = Constants.CreateTableSuccess;
            return true;


        }

        public bool DropTable(string tableName) // Maialen
        {
            //DEADLINE 1.B: Delete the table with the given name. If the table doesn't exist, return false and set LastErrorMessage
            //If everything goes ok, return true and set LastErrorMessage with the appropriate success message (Check Constants.cs)
            Table table = TableByName(tableName);
           
            if(table==null)
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return false;
            }
           
            Tables.Remove(table);
            LastErrorMessage = Constants.DropTableSuccess;
            
            return true;
            
        }

        public bool Insert(string tableName, List<string> values) // Maialen
        {
            //DEADLINE 1.B: Insert a new row to the table. If it doesn't exist return false and set LastErrorMessage appropriately
            //If everything goes ok, set LastErrorMessage with the appropriate success message (Check Constants.cs)
            
            Table table = TableByName(tableName);
            
            if (table == null)
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return false;
            }

            bool success = table.Insert(values);
            
            if(success)
            {
                LastErrorMessage=Constants.InsertSuccess;
                return true;
            }
            else
            {
                LastErrorMessage = Constants.ColumnCountsDontMatch;
                return false;
            }
           
            
        }

        public Table Select(string tableName, List<string> columns, Condition condition) // Endika
        {
            //DEADLINE 1.B: Return the result of the select. If the table doesn't exist return null and set LastErrorMessage appropriately (Check Constants.cs)
            //If any of the requested columns doesn't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return the table
            Table table= TableByName(tableName);
            if (table == null)
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return null;
            }
            if(columns == null || columns.Count==0)
            {
                columns = new List<string>();
                for (int i=0; i<table.NumColumns(); i++)
                {
                    columns.Add(table.GetColumn(i).Name);
                }
            }
            foreach (string col in columns)
            {
                if (table.ColumnIndexByName(col)==-1)
                {
                    LastErrorMessage=Constants.ColumnDoesNotExistError;
                    return null;
                }
            }
            Table result = table.Select(columns, condition);
            return result;
        }

        public bool DeleteWhere(string tableName, Condition columnCondition) // Unai
        {
            //DEADLINE 1.B: Delete all the rows where the condition is true. 
            //If the table or the column in the condition don't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return true

            // Check for table
            Table selectedTable = TableByName(tableName);

            if (selectedTable == null)
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return false;
            }

            // Check for column
            if (selectedTable.ColumnIndexByName(columnCondition.ColumnName) == -1)
            {
                LastErrorMessage = Constants.ColumnDoesNotExistError;
                return false;
            }

            // Final removal
            for (int i = selectedTable.NumRows() - 1; i >= 0; i--)
            {
                if (selectedTable.GetRow(i).IsTrue(columnCondition))
                {
                    selectedTable.DeleteIthRow(i);
                }
            }

            return true;
        }

        public bool Update(string tableName, List<SetValue> columnNames, Condition columnCondition) // Aitana
        {
            //DEADLINE 1.B: Update in the given table all the rows where the condition is true using the SetValues
            //If the table or the column in the condition don't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return true

            Table table = TableByName(tableName);
            if (table == null)
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return false;
            }

            if (columnCondition == null ||
                table.ColumnIndexByName(columnCondition.ColumnName) == -1)
            {
                LastErrorMessage = Constants.ColumnDoesNotExistError;
                return false;
            }

            
            table.Update(columnNames, columnCondition);

            return true;
        }

        private const string tbl=".tbl";
        private const string Delimiter = "--";

        
        public bool Save(string databaseName) // Endika
        {
            //DEADLINE 1.C: Save this database to disk with the given name
            //If everything goes ok, return true, false otherwise.
            //DEADLINE 5: Save the SecurityManager so that it can be loaded with the database in Load()
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), databaseName);
                Directory.CreateDirectory(path);
                foreach (Table table in Tables)
                {
                    string fileName = Path.Combine(path, table.Name + tbl);
                    using (TextWriter writer = File.CreateText(fileName))
                    {
                        for (int i = 0; i < table.NumColumns(); i++)
                        {
                            writer.WriteLine(table.GetColumn(i).AsText());
                        }
                        writer.WriteLine(Delimiter);
                        for (int i = 0; i < table.NumRows(); i++)
                        {
                            writer.WriteLine(table.GetRow(i).AsText());
                        }
                    }
                }
                SecurityManager.Save(Path.Combine(path, "security.dat"));
                return true;
            }
            catch (Exception ex)
            {
                    LastErrorMessage = Constants.Error + ex.Message;
                    return false;
            }
            
        }

        public static Database Load(string databaseName, string username, string password) // Endika
        {
            //DEADLINE 1.C: Load the (previously saved) database of name databaseName
            //If everything goes ok, return the loaded database (a new instance), null otherwise.
            //DEADLINE 5: When the Database object is created, set the username (create a new method if you must)
            //After loading the database, load the SecurityManager and check the password is correct. If it's not, return null. If it is return the database
            try
            {
                Database db = new Database();
                db.m_username = username;
                string path = Path.Combine(Directory.GetCurrentDirectory(), databaseName);
                if (!Directory.Exists(path)) 
                {
                    return null;
                }
                foreach (var filePath in Directory.GetFiles(path, "*"+tbl))
                {
                    using (TextReader reader = File.OpenText(filePath))
                    {
                        List<ColumnDefinition> columns = new List<ColumnDefinition>();
                        string line;
                        while ((line = reader.ReadLine()) != null&&line!=Delimiter)
                        {
                            columns.Add(ColumnDefinition.Parse(line));
                        }
                        string tableName = Path.GetFileNameWithoutExtension(filePath);
                        db.CreateTable(tableName, columns);
                        Table table = db.TableByName(tableName);
                        if (table == null)
                        {
                            db.LastErrorMessage=Constants.TableDoesNotExistError;
                            return null;
                        }
                        if (line==Delimiter)
                        {
                            while ((line = reader.ReadLine()) != null)
                            {
                                Row row = Row.Parse(columns, line);
                                table.Insert(row.Values);
                            }
                        }
                    }
                }
                string secFile = Path.Combine(path, "security.dat");
                if (!File.Exists(secFile)) 
                {
                    db.SecurityManager = new Manager("system");
                    return db; 
                }
                db.SecurityManager = Manager.Load(secFile, username);
                if(db.SecurityManager == null||!db.SecurityManager.IsPasswordCorrect(username, password))
                {
                    return null;
                }
                return db;
            }
            catch(Exception ex) 
            {
                return null;
            }
        }

        public string ExecuteMiniSQLQuery(string query)
        {
            //Parse the query
            MiniSqlQuery miniSQLQuery = MiniSQLParser.Parse(query);

            //If the parser returns null, there must be a syntax error (or the parser is failing)
            if (miniSQLQuery == null)
                return Constants.SyntaxError;

            //Once the query is parsed, we run it on this database
            return miniSQLQuery.Execute(this);
        }


        public bool IsUserAdmin()
        {
            return SecurityManager.IsUserAdmin();
        }





        //All these methods are ONLY FOR TESTING. Use them to simplify creating unit tests:
        public const string AdminUsername = "admin";
        public const string AdminPassword = "adminPassword";
        public static Database CreateTestDatabase()
        {
            Database database = new Database(AdminUsername, AdminPassword);

            database.Tables.Add(Table.CreateTestTable());

            return database;
        }

        public void AddTuplesForTesting(string tableName, List<List<string>> rows)
        {
            Table table = TableByName(tableName);
            foreach (List<string> row in rows)
            {
                table.Insert(row);
            }
        }

        public void CheckForTesting(string tableName, List<List<string>> rows)
        {
            Table table = TableByName(tableName);

            table.CheckForTesting(rows);
        }
    }
}
    
    



