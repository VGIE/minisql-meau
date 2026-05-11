using DbManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace DbManager.Network
{
    public static class XmlDeserializer
    {
        // Aitana
        public static bool ParseOpen(string command, out string database, out string username, out string password)
        {
            //TODO DEADLINE 6: Try to parse the xml command using the specified xml format (eGela)
            //Return true if 'command' is an Open statement, false otherwise. If true, set the value of database, username and password


            database = null;
            username = null;
            password = null;

            string pattern = @"^<Open\s+Database=""(?<Database>[^""]+)""\s+User=""(?<User>[^""]+)""\s+Password=""(?<Password>[^""]+)""\s*/>$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(command);

            if (match.Success)
            {
                database = match.Groups["Database"].Value;
                username = match.Groups["User"].Value;
                password = match.Groups["Password"].Value;
                return true;
            }


            return false;
        }

        // Aitana
        public static bool ParseOpenCreateAnswer(string answer, out string error)
        {
            //TODO DEADLINE 6: Try to parse the answer to an Open/Create command.
            //Return true if 'command' is equal to XmlSerializer.OpenCreateSuccess
            //If it is an error (<Error>...</Error>), return false and set 'error' with the error message


            error = null;

            if (string.IsNullOrEmpty(answer))
                return false;

            if (answer == XmlSerializer.OpenCreateSuccess)
                return true;

            string patternError = @"^<Error>(?<Error>.+)</Error>$";
            Regex regexError = new Regex(patternError, RegexOptions.IgnoreCase);
            Match matchError = regexError.Match(answer);

            if (matchError.Success)
            {
                error = matchError.Groups["Error"].Value;
                return false;
            }

            return false;
        }

        // Maialen
        public static bool ParseCreate(string command, out string database, out string username, out string password)
        {
            //TODO DEADLINE 6: Try to parse a Create xml command using the specified xml format (eGela)
            //Return true if 'command' is a Create statement, false otherwise. If true, set the value of database, username and password

            database = null;
            username = null;
            password = null;
            try
            {
                if (string.IsNullOrEmpty(command) || !command.Trim().StartsWith("<Create"))
                {
                    return false;
                }
                XElement xml = XElement.Parse(command);
                if (xml.Name.LocalName == "Create")
                {
                    database = (string)xml.Attribute("Database");
                    username = (string)xml.Attribute("User");
                    password = (string)xml.Attribute("Password");
                    return !string.IsNullOrEmpty(database);
                }
            }
            catch
            {
                return false;
            }
            return false;
        }





        // Endika
        public static bool ParseQuery(string answer, out string query)
        {
            //TODO DEADLINE 6: Try to parse a Query xml command using the specified xml format (eGela)
            //Return true if 'command' is a Query statement, false otherwise. If true, set the value of query with the content of the command

            query = null;
            if(string.IsNullOrEmpty(answer))
            {
                return false;
            }
            string pattern= @"^<Query>(?<Query>[\s\S]*)</Query>$";
            Regex regex= new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(answer);
            if (match.Success)
            {
                query=match.Groups["query"].Value;
                return true;
            }
            return false;
        }

        // Endika
        public static bool ParseQueryAnswer(string answer, out string answerContent)
        {
            //TODO DEADLINE 6: Try to parse the answer to a Query command.
            //Return true if 'command' does not contain an error inside (<Error>...</Error>)
            //If it is an error (<Error>...</Error>), return false and set 'answerContent' with the error message

            answerContent = null;
            if (string.IsNullOrEmpty(answer))
            {
                return false;
            }
            string patternError = @"^<Error>(?<Error>[\s\S]*)</Error>$";
            Regex regexError = new Regex(patternError, RegexOptions.IgnoreCase);
            Match matchError = regexError.Match(answer);
            if (matchError.Success)
            {
                answerContent = matchError.Groups["Error"].Value;
                return false;
            }
            answerContent = answer;
            return true;
        }

        public static bool IsCloseCommand(string command)
        {
            return command == XmlSerializer.CloseConnection;
        }
    }
}
