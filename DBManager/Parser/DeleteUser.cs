using DbManager.Parser;
using DbManager.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbManager
{
 
    // Unai
    public class DeleteUser : MiniSqlQuery
    {
        public string Username { get; private set; }

        public DeleteUser(string username)
        {
            //TODO DEADLINE 4: Initialize member variables
            this.Username = username;
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, UserDoesNotExistError, DeleteUserSuccess

            if (!database.SecurityManager.IsUserAdmin())
            {
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            }

            Profile profile = database.SecurityManager.ProfileByUser(Username);

            if (profile == null)
            {
                return Constants.UserDoesNotExistError;
            }

            User userToRemove = null;
            foreach (User u in profile.Users)
            {
                if (u.Username == Username)
                {
                    userToRemove = u;
                    break;
                }
            }

            if (userToRemove != null)
            {
                profile.Users.Remove(userToRemove);
                return Constants.DeleteUserSuccess;
            }

            return Constants.UserDoesNotExistError;

        }

    }
}
