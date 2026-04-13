using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Security
{
    public class Manager
    {
        public List<Profile> Profiles { get; private set; } = new List<Profile>();

        private string m_username;
        public Manager(string username)
        {
            m_username = username;
        }

        // Aitana
        public bool IsUserAdmin()
        {
            //TODO DEADLINE 5: Return true if the user logged-in (m_username) is the admin, false otherwise
            
           
            Profile profile = ProfileByUser(m_username);

            if (profile != null)
            {
                if (profile.Name.Equals(Profile.AdminProfileName))
                    return true;
            }
            return false;
    
        }




        // Maialen
        public bool IsPasswordCorrect(string username, string password)
        {
            //TODO DEADLINE 5: Return true if the user's password is correct. The given password should be encrypted before comparing with the saved one
            
            User obj = UserByName(username);

            if (obj.EncryptedPassword == Encryption.Encrypt(password))
            {
               return true;
            }
            else
            {
                return false;
            }
            
            
        }

        // Endika
        public void GrantPrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Add this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing
            if(profileName==null || table==null)
            {
                return ;
            }
            Profile profile = ProfileByUser(profileName);
            if (profile != null)
            {
                profile.GrantPrivilege(table, privilege);
            }     
        }

        // Unai
        public void RevokePrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Remove this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing

            if (profileName == null || table == null)
            {
                return;
            }

            Profile profile = ProfileByName(profileName);

            if (profile != null)
            {
                profile.GrantPrivilege(table, privilege);
            }
        }

        // Aitana
        public bool IsGrantedPrivilege(string username, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Return true if the username has this privilege on this table. False otherwise (also in case of error)

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(table))
            {
                return false;
            }

            Profile profile = ProfileByUser(username);
            if (profile == null)
            {
                return false;
            }

            if (profile.Name == Profile.AdminProfileName)
            {
                return true;
            }

            return profile.IsGrantedPrivilege(table, privilege);



        }

        // Aitana
        public void AddProfile(Profile profile)
        {
            //TODO DEADLINE 5: Add this profile


            if (IsUserAdmin())
            {
                if (profile != null)
                {
                    if (ProfileByName(profile.Name) == null)
                    {
                        Profiles.Add(profile);
                    }
                }
            }


        }

        // Maialen
        public User UserByName(string username)
        {
            //TODO DEADLINE 5: Return the user by name. If it doesn't exist, return null
             foreach(Profile p in Profiles)
            {
                foreach(User u in p.Users)
                {
                    if (username.Equals(u.Username))
                    {
                        return u;
                    }
                }
            }
            return null;
            
        }

        // Maialen
        public Profile ProfileByName(string profileName)
        {
            //TODO DEADLINE 5: Return the profile by name. If it doesn't exist, return null
           foreach(var p in Profiles)
            {
                if(p.Name==profileName)
                    return p;
            }
            return null;
            
        }

        // Unai
        public Profile ProfileByUser(string username)
        {
            //TODO DEADLINE 5: Return the profile by user. If the user doesn't exist, return null
            
            return null;
            
        }

        // Aitana
        public bool RemoveProfile(string profileName)
        {
            //TODO DEADLINE 5: Remove this profile
            
  
            if (IsUserAdmin())
            {
                Profile profile = ProfileByName(profileName);

                if (profile != null)
                {

                    Profiles.Remove(profile);
                    return true;
                    
                }
            }

            return false;

        }

        // Unai
        public static Manager Load(string databaseName, string username)
        {
            //TODO DEADLINE 5: Load all the profiles and users saved for this database. The Manager instance should be created with the given username
            
            return null;
            
        }

        // Endika
        public void Save(string databaseName)
        {
            //TODO DEADLINE 5: Save all the profiles and users/passwords created for this database.
            
        }
    }
}
