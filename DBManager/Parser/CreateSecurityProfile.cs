using DbManager.Parser;
using DbManager.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbManager
{
    
    // Aitana
    public class CreateSecurityProfile : MiniSqlQuery
    {
        public string ProfileName { get; set; }

        public CreateSecurityProfile(string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
            this.ProfileName = profileName;

        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, CreateSecurityProfileSuccess

            if (!database.IsUserAdmin())
            {
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            }

            database.SecurityManager.AddProfile(new Profile 
            { 
                Name = this.ProfileName 
            });

            return Constants.CreateSecurityProfileSuccess;


        }

    }
}
