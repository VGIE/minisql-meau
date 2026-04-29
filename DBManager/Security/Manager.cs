using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
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
                if (profile.Name == Profile.AdminProfileName)
                    return true;
            }
            return false;

        }




        // Maialen
        public bool IsPasswordCorrect(string username, string password)
        {
            //TODO DEADLINE 5: Return true if the user's password is correct. The given password should be encrypted before comparing with the saved one

            User obj = UserByName(username);

            if (obj == null)
            {
                return false;
            }
            return obj.EncryptedPassword == Encryption.Encrypt(password);

        }




        // Endika
        public void GrantPrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Add this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing

            if (IsUserAdmin())
            {
                Profile profile = ProfileByName(profileName);


                if (profileName == null || table == null)
                {
                    return;
                }
                if (profile != null)
                {
                    profile.GrantPrivilege(table, privilege);
                }
            }
        }

        // Unai
        public void RevokePrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Remove this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing
            if (!IsUserAdmin()) return;

            if (IsUserAdmin())
            {
                Profile profile = ProfileByName(profileName);

                if (profileName == null || table == null)
                {
                    return;
                }


                if (profile != null)
                {
                    profile.RevokePrivilege(table, privilege);
                }
            }
        }

        // Aitana
        public bool IsGrantedPrivilege(string username, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Return true if the username has this privilege on this table. False otherwise (also in case of error)

            
            
            
            if (username == null) {
              return false; 
            }

            if (ProfileByUser(username) != null)
            {

                Profile profile = ProfileByUser(username);

           

            if (profile.Name == Profile.AdminProfileName)
            {
                return true;
            }

            return profile.IsGrantedPrivilege(table, privilege);
            }
             return false;
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

            foreach(var p in Profiles)
            {
                foreach (User us in p.Users)
                {
                    if (us.Username == username)
                    {
                        return us;
                    }
                }
            }
            return null;

        }

        // Maialen
        public Profile ProfileByName(string profileName)
        {
            //TODO DEADLINE 5: Return the profile by name. If it doesn't exist, return null
            if (string.IsNullOrEmpty(profileName))
            {
                return null;
            }
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
            if (username == null)
            {
                return null;
            }

            Profile profile = null;

            foreach (Profile p in Profiles)
            {
                foreach (User u in p.Users)
                {
                    if (u.Username == username)
                    {
                        profile = p;
                    }
                }
            }

            if (profile == null)
            {
                return null;
            }

            return profile;

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
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), databaseName, "security.json");
            Manager manager = new Manager(username);

            if (!File.Exists(filePath)) return manager;

            try
            {
                string jsonString = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions
                {
                    IncludeFields = true,
                    PropertyNameCaseInsensitive = true
                };

                List<Profile> loadedProfiles = JsonSerializer.Deserialize<List<Profile>>(jsonString, options);
                if (loadedProfiles != null)
                {
                    JsonNode root = JsonNode.Parse(jsonString);
                    if (root != null)
                    {
                        JsonArray profilesArray = root.AsArray();

                        for (int i = 0; i < loadedProfiles.Count; i++)
                        {
                            var profileNode = profilesArray[i];
                            if (profileNode["PrivilegesOn"] != null)
                            {
                                var privilegesObject = profileNode["PrivilegesOn"].AsObject();
                                foreach (var prop in privilegesObject)
                                {
                                    string table = prop.Key;
                                    var privilegesArray = prop.Value.AsArray();
                                    foreach (var privNode in privilegesArray)
                                    {
                                        int privVal = privNode.GetValue<int>();
                                        loadedProfiles[i].GrantPrivilege(table, (Privilege)privVal);
                                    }
                                }
                            }
                        }
                    }

                    manager.Profiles.Clear();
                    manager.Profiles.AddRange(loadedProfiles);
                }
                return manager;
            }
            catch (Exception)
            {
                return manager;
            }

        }

        // Endika
        public void Save(string databaseName)
        {
            //TODO DEADLINE 5: Save all the profiles and users/passwords created for this database.
            try
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), databaseName);
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                string filePath = Path.Combine(folder, "security.json");

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    IncludeFields = true
                };

                string jsonString = JsonSerializer.Serialize(this.Profiles, options);
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
