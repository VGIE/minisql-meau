using DbManager.Parser;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DbManager
{
    public class MiniSQLParser
    {
        // Unai
        public static MiniSqlQuery Parse(string miniSQLQuery)
        {
            //TODO DEADLINE 2

            RegexOptions options = RegexOptions.Singleline;

            const string selectPattern = @"^SELECT\s+(?<columns>(?:\w+|\*)(?:,(?:\w+|\*))*)\s+FROM\s+(?<table>\w+)(?:\s+WHERE\s+(?<condition>\S.+))?\s*$";
            
            const string insertPattern = @"^INSERT\s+INTO\s+(?<table>\w+)\s+VALUES\s*\(\s*(?<values>(?:'[^']*'|-?[0-9]+(?:\.[0-9]+)?)(?:,(?:'[^']*'|-?[0-9]+(?:\.[0-9]+)?))*)\s*\)\s*$";
            
            const string dropTablePattern = @"^DROP\s+TABLE\s+(?<table>\w+)\s*$";

            const string createTablePattern = @"^CREATE\s+TABLE\s+(?<table>\w+)\s+\(\s*(?<columns>\w+\s+(?:INT|DOUBLE|STRING)(?:,\w+\s+(?:INT|DOUBLE|STRING))*)\)\s*$";

            const string updateTablePattern = @"^UPDATE\s+(?<table>\w+)\s+SET\s+(?<assignments>\w+=(?:'[^']*'|-?[0-9]+(?:\.[0-9]+)?)(?:,\w+=(?:'[^']*'|-?[0-9]+(?:\.[0-9]+)?))*)\s+WHERE\s+(?<condition>\S.+)\s*$";
            
            const string deletePattern = @"^DELETE\s+FROM\s+(?<table>\w+)\s+WHERE\s+(?<condition>\S.+)\s*$";
            

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
            const string conditionPattern = @"^\s*(?<colname>\w+)(?<operator>[<>=]+)(?<value>'[^']*'|-?[0-9]+(?:\.[0-9]+)?)\s*$";
            const string columnDefinitionPattern = @"^\s*(?<colname>\w+)\s+(?<type>STRING|INT|DOUBLE)\s*$";
            const string setValuePattern = @"^(?<colname>\w+)=(?<value>.+)\s*$";
            
            // SELECT
            Match selectMatch = Regex.Match(miniSQLQuery, selectPattern, options);
            if (selectMatch.Success)
            {
                string table = selectMatch.Groups["table"].Value;

                List<string> columns = CommaSeparatedNames(selectMatch.Groups["columns"].Value);
                if (columns == null) return null;

                string conditionString = selectMatch.Groups["condition"].Value;
                Condition condition = null;

                if (!string.IsNullOrEmpty(conditionString))
                {
                    Match conditionMatch = Regex.Match(conditionString, conditionPattern, options);

                    if (conditionMatch.Success)
                    {
                        string colname = conditionMatch.Groups["colname"].Value;
                        string operatorString = conditionMatch.Groups["operator"].Value;
                        string valueString = conditionMatch.Groups["value"].Value;

                        condition = new Condition(colname, operatorString, StripQuote(valueString));
                    }
                    else return null;
                }

                return new Select(table, columns, condition);
            }

            // INSERT
            Match insertMatch = Regex.Match(miniSQLQuery, insertPattern, options);
            if (insertMatch.Success)
            {
                string tableString = insertMatch.Groups["table"].Value;
                List<string> values = CommaSeparatedNames(insertMatch.Groups["values"].Value);
                if (values == null) return null;

                for (int i = 0; i < values.Count; i++)
                {
                    values[i] = StripQuote(values[i]);
                }
                
                return new Insert(tableString, values);
            }

            Match dropTableMatch = Regex.Match(miniSQLQuery, dropTablePattern, options);
            if (dropTableMatch.Success)
            {
                string tableString = dropTableMatch.Groups["table"].Value;

                return new DropTable(tableString);
            }

            // CREATE TABLE
            Match createTableMatch = Regex.Match(miniSQLQuery, createTablePattern, options);
            if (createTableMatch.Success)
            {
                string tableString = createTableMatch.Groups["table"].Value;
                List<string> columnsString = CommaSeparatedNames(createTableMatch.Groups["columns"].Value);
                if (columnsString == null) return null;
                List<ColumnDefinition> columns = new();

                foreach (string column in columnsString)
                {
                    ColumnDefinition colDef = null;
                    Match columnDefMatch = Regex.Match(column, columnDefinitionPattern, options);

                    if (columnDefMatch.Success)
                    {
                        string colName = columnDefMatch.Groups["colname"].Value;
                        string colType = columnDefMatch.Groups["type"].Value;
                        ColumnDefinition.DataType colTypeEnum;
                        string STRING = "STRING";
                        string DOUBLE = "DOUBLE";
                        string INT = "INT";

                        if (colType == STRING)
                        {
                            colTypeEnum = ColumnDefinition.DataType.String;
                        }
                        else if (colType == DOUBLE)
                        {
                            colTypeEnum = ColumnDefinition.DataType.Double;
                        }
                        else if (colType == INT)
                        {
                            colTypeEnum = ColumnDefinition.DataType.Int;
                        }
                        else
                        {
                            return null;
                        }

                        colDef = new ColumnDefinition(colTypeEnum, colName);

                        columns.Add(colDef);
                    }
                    else
                    {
                        return null;
                    }
                }

                return new CreateTable(tableString, columns);
            }

            // UPDATE TABLE
            Match updateTableMatch = Regex.Match(miniSQLQuery, updateTablePattern, options);
            if (updateTableMatch.Success)
            {
                string tableString = updateTableMatch.Groups["table"].Value;
                List<string> assignmentsString = CommaSeparatedNames(updateTableMatch.Groups["assignments"].Value);
                if (assignmentsString == null) return null;
                string conditionString = updateTableMatch.Groups["condition"].Value;

                List<SetValue> assignmentsSetValue = new();

                foreach (string assignment in assignmentsString)
                {
                    SetValue setValue = null;
                    Match setValueMatch = Regex.Match(assignment, setValuePattern, options);

                    if (setValueMatch.Success)
                    {
                        string colName = setValueMatch.Groups["colname"].Value;
                        string value = StripQuote(setValueMatch.Groups["value"].Value);

                        setValue = new SetValue(colName, value);

                        assignmentsSetValue.Add(setValue);
                    }
                    else return null;
                }

                Condition condition = null;

                if (!string.IsNullOrEmpty(conditionString))
                {
                    Match conditionMatch = Regex.Match(conditionString, conditionPattern, options);

                    if (conditionMatch.Success)
                    {
                        string colname = conditionMatch.Groups["colname"].Value;
                        string operatorString = conditionMatch.Groups["operator"].Value;
                        string valueString = conditionMatch.Groups["value"].Value;

                        if (valueString.StartsWith("'") && !valueString.EndsWith("'"))
                        {
                            return null;
                        }

                        condition = new Condition(colname, operatorString, StripQuote(valueString));
                    }
                    else return null;
                }

                return new Update(tableString, assignmentsSetValue, condition);
            }

            // DELETE
            Match deleteMatch = Regex.Match(miniSQLQuery, deletePattern, options);
            if (deleteMatch.Success)
            {
                string tableString = deleteMatch.Groups["table"].Value;
                string conditionString = deleteMatch.Groups["condition"].Value;

                Condition condition = null;

                if (!string.IsNullOrEmpty(conditionString))
                {
                    Match conditionMatch = Regex.Match(conditionString, conditionPattern, options);

                    if (conditionMatch.Success)
                    {
                        string colname = conditionMatch.Groups["colname"].Value;
                        string operatorString = conditionMatch.Groups["operator"].Value;
                        string valueString = conditionMatch.Groups["value"].Value;

                        if (valueString.StartsWith("'") && !valueString.EndsWith("'"))
                        {
                            return null;
                        }

                        condition = new Condition(colname, operatorString, StripQuote(valueString));
                    }
                    else return null;
                }

                return new Delete(tableString, condition);
            }

            // CREATE SECURITY PROFILE
            Match createSecProfileMatch = Regex.Match(miniSQLQuery, createSecurityProfilePattern, options);
            if (createSecProfileMatch.Success)
            {
                string secProfile = createSecProfileMatch.Groups["secprofile"].Value;

                return new CreateSecurityProfile(secProfile);
            }

            // DROP SECURITY PROFILE
            Match dropSecProfileMatch = Regex.Match(miniSQLQuery, dropSecurityProfilePattern, options);
            if (dropSecProfileMatch.Success)
            {
                string secProfile = dropSecProfileMatch.Groups["secprofile"].Value;

                return new DropSecurityProfile(secProfile);
            }

            // GRANT
            Match grantMatch = Regex.Match(miniSQLQuery, grantPattern, options);
            if (grantMatch.Success)
            {
                string privilegeString = grantMatch.Groups["privilege"].Value;
                string tableString = grantMatch.Groups["table"].Value;
                string secprofileString = grantMatch.Groups["secprofile"].Value;

                return new Grant(privilegeString, tableString, secprofileString);
            }

            // REVOKE
            Match revokeMatch = Regex.Match(miniSQLQuery, revokePattern, options);
            if (revokeMatch.Success)
            {
                string privilegeString = revokeMatch.Groups["privilege"].Value;
                string tableString = revokeMatch.Groups["table"].Value;
                string secprofileString = revokeMatch.Groups["secprofile"].Value;

                return new Revoke(privilegeString, tableString, secprofileString);
            }

            // ADD USER
            Match addUserMatch = Regex.Match(miniSQLQuery, addUserPattern, options);
            if (addUserMatch.Success)
            {
                string userString = addUserMatch.Groups["user"].Value;
                string passString = addUserMatch.Groups["password"].Value;
                string secprofileString = addUserMatch.Groups["secprofile"].Value;

                return new AddUser(userString, passString, secprofileString);
            }

            // DELETE USER
            Match deleteUserMatch = Regex.Match(miniSQLQuery, deleteUserPattern, options);
            if (deleteUserMatch.Success)
            {
                string userString = deleteUserMatch.Groups["user"].Value;

                return new DeleteUser(userString);
            }

            return null;
        }

        static List<string> CommaSeparatedNames(string text)
        {
            string[] textParts = text.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
            List<string> commaSeparator = new List<string>();
            for(int i=0; i < textParts.Length; i++)
            {
                commaSeparator.Add(textParts[i].Trim());
            }
            return commaSeparator;
        }

        static string StripQuote(string value)
        {
            if (value != null && value.StartsWith("'") && value.EndsWith("'") && value.Length >= 2)
            {
                return value.Substring(1, value.Length - 2);
            }
            return value;
        }
    }
}
