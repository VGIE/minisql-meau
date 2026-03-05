using DbManager.Parser;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DbManager
{
    public class MiniSQLParser
    {
        // Unai
        public static MiniSqlQuery Parse(string miniSQLQuery)
        {
            //TODO DEADLINE 2
            const string selectPattern = @"^SELECT\s+(?<columns>.+)\s+FROM\s+(?<table>\w+)(?:\s+WHERE\s+(?<condition>.+))?\s*$";
            
            const string insertPattern = @"^INSERT\s+INTO\s+(?<table>\w+)\s+VALUES\s*\((?<values>'[^']*'(?:,\s*'[^']*')*)\)\s*$";
            
            const string dropTablePattern = @"^DROP\s+TABLE\s+(?<table>\w+)\s*$";
            
            //Note: The parsing of CREATE TABLE should accept empty columns "()"
            //And then, an execution error should be given if a CreateTable without columns is executed
            const string createTablePattern = @"^CREATE\s+TABLE\s+(?<table>\w+)\s*\(\s*(?<columns>\w+\s+\w+(?:,\w+\s+\w+)*)\s*\)\s*$";
            
            const string updateTablePattern = @"^UPDATE\s+(?<table>\w+)\s+SET\s+(?<assignments>\w+\s*=\s*(?:'[^']*'|[^,\s]+)(?:,\s*\w+\s*=\s*(?:'[^']*'|[^,\s]+))*)\s+WHERE\s+(?<condition>.+)\s*$";
            
            const string deletePattern = @"^DELETE\s+FROM\s+(?<table>\w+)\s+WHERE\s+(?<condition>.+)\s*$";
            

            //TODO DEADLINE 4
            const string createSecurityProfilePattern = @"^CREATE\s+SECURITY\s+PROFILE\s+(?<secprofile>[a-zA-Z]+)\s*$";
            
            const string dropSecurityProfilePattern = @"^DROP\s+SECURITY\s+PROFILE\s+(?<secprofile>[a-zA-Z]+)\s*$";

            const string grantPattern = @"^GRANT\s+(?<privilege>DELETE|INSERT|SELECT|UPDATE)\s+ON\s+(?<table>\w+)\s+TO\s+(?<secprofile>[a-zA-Z]+)\s*$";
            
            const string revokePattern = @"^REVOKE\s+(?<privilege>DELETE|INSERT|SELECT|UPDATE)\s+ON\s+(?<table>\w+)\s+TO\s+(?<secprofile>[a-zA-Z]+)\s*$";
            
            const string addUserPattern = @"^ADD\s+USER\s+\((?<user>[a-zA-Z]+),(?<password>[^,]+),(?<secprofile>[a-zA-Z]+)\)\s*$";
            
            const string deleteUserPattern = @"^DELETE\s+USER\s+(?<user>[a-zA-Z]+)\s*$";


            //TODO DEADLINE 2
            //Parse query using the regular expressions above one by one. If there is a match, create an instance of the query with the parsed parameters
            //For example, if the query is a "SELECT ...", there should be a match with selectPattern. We would create and return an instance of Select
            //initialized with the table name, the columns, and (possibly) an instance of Condition.
            //If there is no match, it means there is a syntax error. We will return null.

            //TODO DEADLINE 4
            //Do the same for the security queries (CREATE SECURITY PROFILE, ...)
            const string conditionPattern = @"^(?<colname>.*)\s*(?<operator>[<> =])\s*(?<value>.+)$";
            

            Match selectMatch = Regex.Match(miniSQLQuery, selectPattern);
            if (selectMatch.Success)
            {
                string table = selectMatch.Groups["table"].Value;

                List<string> columns = CommaSeparatedNames(selectMatch.Groups["columns"].Value);

                string conditionString = selectMatch.Groups["condition"].Value;
                Condition condition = null;

                if (!string.IsNullOrEmpty(conditionString))
                {
                    Match conditionMatch = Regex.Match(conditionString, conditionPattern);

                    if (conditionMatch.Success)
                    {
                        string colname = conditionMatch.Groups["colname"].Value;
                        string operatorString = conditionMatch.Groups["operator"].Value;
                        string valueString = conditionMatch.Groups["value"].Value;

                        condition = new Condition(colname, operatorString, valueString);
                    }
                }

                return new Select(table, columns, condition);
            }


            return null;
           
        }

        static List<string> CommaSeparatedNames(string text)
        {
            string[] textParts = text.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
            List<string> commaSeparator = new List<string>();
            for(int i=0; i < textParts.Length; i++)
            {
                commaSeparator.Add(textParts[i]);
            }
            return commaSeparator;
        }
        
    }
}
