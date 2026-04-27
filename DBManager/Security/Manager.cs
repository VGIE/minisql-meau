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
                if(profile.Name == Profile.AdminProfileName)
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
            if (!IsUserAdmin()) return;

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

        // Unai
        public void RevokePrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Remove this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing
            if (!IsUserAdmin()) return;

            if (profileName == null || table == null)
            {
                return;
            }

            Profile profile = ProfileByName(profileName);

            if (profile != null)
            {
                profile.RevokePrivilege(table, privilege);
            }
        }

        // Aitana
        public bool IsGrantedPrivilege(string username, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Return true if the username has this privilege on this table. False otherwise (also in case of error)

            if (!IsUserAdmin() && username != m_username)
            {
                return false;
            }

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(table))
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

            if (username == null)
            {
                return null;
            }
            foreach (Profile p in Profiles)
            {
                foreach (User u in p.Users)
                {
                    if (u.Username.Equals(username))
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
            if (string.IsNullOrEmpty(profileName))
            {
                return null;
            }
            foreach (Profile p in Profiles)
            {
                if (p.Name.Equals(profileName))
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
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), databaseName, "security.dat");

                if (!File.Exists(path)) return new Manager(username);

                string jsonString = File.ReadAllText(path);

                List<Profile> loadedProfiles = JsonSerializer.Deserialize<List<Profile>>(jsonString);

                Manager manager = new Manager(username);
                if (loadedProfiles != null)
                {
                    manager.Profiles = loadedProfiles;

                    JsonArray jsonArray = JsonNode.Parse(jsonString).AsArray();
                    for (int i = 0; i < loadedProfiles.Count; i++)
                    {
                        var privsNode = jsonArray[i]?["PrivilegesOn"]?.AsObject();
                        if (privsNode != null)
                        {
                            foreach (var tableNode in privsNode)
                            {
                                string tableName = tableNode.Key;
                                var privArray = tableNode.Value?.AsArray();
                                if (privArray != null)
                                {
                                    foreach (var privNode in privArray)
                                    {
                                        Privilege p = (Privilege)privNode.GetValue<int>();
                                        loadedProfiles[i].GrantPrivilege(tableName, p);
                                    }
                                }
                            }
                        }
                    }

                    if (manager.UserByName(username) == null)
                    {
                        return null;
                    }

                }

                return manager;
            }
            catch (Exception)
            {
                return null;
            }

        }

        // Endika
        public void Save(string databaseName)
        {
            //commit para creacion de rama 
            //TODO DEADLINE 5: Save all the profiles and users/passwords created for this database.
            if (!IsUserAdmin())
            {
                return;
            }

            try
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), databaseName);
                Directory.CreateDirectory(folder);
                string path = Path.Combine(folder, "security.dat");
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    foreach (Profile profile in Profiles)
                    {
                        if (profile == null)
                        {
                            continue;
                        }
                        foreach (User user in profile.Users)
                        {
                            if (user == null)
                            {
                                continue;
                                string privileges = "";
                                if (profile.IsGrantedPrivilege("Users", Privilege.Select))
                                {
                                    if (privileges != "")
                                    {
                                        privileges += "/";
                                    }
                                    privileges += Privilege.Select.ToString();
                                }
                                if (profile.IsGrantedPrivilege("Users", Privilege.Insert))
                                {
                                    if (privileges != "")
                                    {
                                        privileges += "/";
                                    }
                                    privileges += Privilege.Insert.ToString();
                                }
                                if (profile.IsGrantedPrivilege("Users", Privilege.Delete))
                                {
                                    if (privileges != "")
                                    {
                                        privileges += "/";
                                    }
                                    privileges += Privilege.Delete.ToString();
                                }
                                if (profile.IsGrantedPrivilege("Users", Privilege.Update))
                                {
                                    if (privileges != "")
                                    {
                                        privileges += "/";
                                    }
                                    privileges += Privilege.Update.ToString();
                                }
                                if (privileges != "")
                                {
                                    sw.WriteLine(user.Username + "," + user.EncryptedPassword + "," + profile.Name + ",Users," + privileges);
                                }
                            }
                        }
                    }
                }
                ;
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
