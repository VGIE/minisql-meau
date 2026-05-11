using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;
using DbManager.Security;

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
            if (database == null)
            {
                return Constants.Error;
            }
            if (!database.SecurityManager.IsUserAdmin())
            {
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            }
            if (database.SecurityManager.ProfileByName(ProfileName) == null)
            {
                return Constants.SecurityProfileDoesNotExistError;
            }
            DbManager.Security.Privilege privilege;
            string upperPrivilege = PrivilegeName.ToUpper();
            string select = "SELECT";
            string insert = "INSERT";
            string update = "UPDATE";
            string delete = "DELETE";

            if (upperPrivilege == select)
            {
                privilege = DbManager.Security.Privilege.Select;
            }
            else if (upperPrivilege == insert)
            {
                privilege = DbManager.Security.Privilege.Insert;
            }
            else if (upperPrivilege == update)
            {
                privilege = DbManager.Security.Privilege.Update;
            }
            else if (upperPrivilege == delete)
            {
                privilege = DbManager.Security.Privilege.Delete;
            }
            else
            {
                return Constants.PrivilegeDoesNotExistError;
            }

            if (database.SecurityManager.IsGrantedPrivilege(ProfileName, TableName, privilege))
            {
                return Constants.ProfileAlreadyHasPrivilege;
            }
            database.SecurityManager.GrantPrivilege(ProfileName, TableName, privilege);
            return Constants.GrantPrivilegeSuccess;
            

        }

    }
}
