using DbManager.Parser;
using DbManager.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbManager
{
 
    // Aitana
    public class Revoke : MiniSqlQuery
    {
        public string PrivilegeName { get; set; }
        public string TableName { get; set; }
        public string ProfileName { get; set; }

        public Revoke(string privilegeName, string tableName, string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
            PrivilegeName = privilegeName;
            TableName = tableName;
            ProfileName = profileName;

        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, RevokePrivilegeSuccess, 

            if (!database.SecurityManager.IsUserAdmin())
            {
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            }

            Profile profile = database.SecurityManager.ProfileByName(ProfileName);

            if (profile == null)
            {
                return Constants.SecurityProfileDoesNotExistError;
            }

            Privilege privilege;
            try
            {
                privilege = PrivilegeUtils.FromPrivilegeName(PrivilegeName);
            }
            catch (Exception)
            {
                return Constants.PrivilegeDoesNotExistError;
            }

            if (!profile.IsGrantedPrivilege(TableName, privilege))
            {
                return Constants.PrivilegeDoesNotExistError;
            }

            database.SecurityManager.RevokePrivilege(ProfileName, TableName, privilege);

            return Constants.RevokePrivilegeSuccess;

        }

    }
}
