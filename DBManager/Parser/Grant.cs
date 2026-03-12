using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;

namespace DbManager
{
 
    // Endika
    public class Grant : MiniSqlQuery
    {
        public string PrivilegeName { get; set; }
        public string TableName { get; set; }
        public string ProfileName { get; set; }

        public Grant(string privilegeName, string tableName, string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
            this.PrivilegeName = privilegeName;
            this.TableName = tableName;
            this.ProfileName = profileName;
            
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, PrivilegeDoesNotExistError, GrantPrivilegeSuccess, ProfileAlreadyHasPrivilege
            /*if (database == null)
            {
                return Constants.Error;
            }
            if (!database.SecurityManager.IsUserAdmin())
            {
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            }
            if(database.SecurityManager.ProfileByName(ProfileName)==null)
            {
                return Constants.SecurityProfileDoesNotExistError;
            }
            DbManager.Security.Privilege privilege;
            if(!Enum.TryParse(PrivilegeName, out privilege))
            {
                return Constants.PrivilegeDoesNotExistError;
            }
            if(database.SecurityManager.IsGrantedPrivilege(ProfileName,TableName, privilege))
            {
                return Constants.ProfileAlreadyHasPrivilege;
            }
            database.SecurityManager.GrantPrivilege(ProfileName, TableName, privilege);
            return Constants.GrantPrivilegeSuccess;*/
            return null;
            
        }

    }
}
