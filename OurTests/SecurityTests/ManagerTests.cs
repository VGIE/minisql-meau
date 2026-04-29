using DbManager;
using DbManager.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace OurTests.SecurityTests
{
    public class ManagerTests
    {
        private Manager manager;

        public ManagerTests(){
            manager = new Manager("admin");
            Profile userProfile = new Profile { Name = "UserPrf" };
            userProfile.Users.Add(new User("admin", "pass123"));
            manager.Profiles.Add(userProfile);
        }
    

        [Fact]
        public void TestRevokePrivilege()
        {
            Manager manager = new Manager("Admin");

            User admin = new User("Admin", "1234");
            Profile adminProfile = new Profile();
            adminProfile.Name = Profile.AdminProfileName;
            adminProfile.Users.Add(admin);
            manager.Profiles.Add(adminProfile);

            string profileName = "Developer";
            string tableName = "Employees";
            Privilege priv = Privilege.Update;

            Profile profile = new Profile();
            profile.Name = profileName;
            manager.Profiles.Add(profile);

            profile.GrantPrivilege(tableName, priv);

            Assert.True(profile.IsGrantedPrivilege(tableName, priv));

            manager.RevokePrivilege(profileName, tableName, priv);

            bool hasPrivilege = profile.IsGrantedPrivilege(tableName, priv);
            Assert.False(hasPrivilege);
        }

        [Fact]
        public void TestRevokePrivilegeNotExists()
        {
            Manager manager = new Manager("Admin");

            manager.RevokePrivilege("RandomProfile", "RandomTable", Privilege.Delete);

            Assert.True(true);
        }

        [Fact]
        public void TestProfileByUser()
        {
            Manager manager = new Manager("Admin");
            string profileName = "Developers";
            string targetUser = "Unai";

            Profile profile = new Profile { Name = profileName };
            User user = new User(targetUser, "pass");
            profile.Users.Add(user);

            manager.Profiles.Add(profile);

            Profile result = manager.ProfileByUser(targetUser);

            Assert.NotNull(result);
            Assert.Equal(profileName, result.Name);
        }

        [Fact]
        public void TestProfileByUserUserDoesNotExist()
        {
            Manager manager = new Manager("Admin");
            Profile profile = new Profile { Name = "Unai" };
            profile.Users.Add(new User("existingUser", "pass"));
            manager.Profiles.Add(profile);

            Profile result = manager.ProfileByUser("nonExistentUser");

            Assert.Null(result);
        }

        [Fact]
        public void TestProfileByUserNullUsername()
        {
            Manager manager = new Manager("Admin");

            Profile result = manager.ProfileByUser(null);

            Assert.Null(result);
        }

        [Fact]
        public void TestLoad()
        {
            string dbName = "TestDatabase";
            string testUser = "admin_user";

            Manager managerToSave = new Manager(testUser);
            Profile profileToSave = new Profile { Name = "AdminProfile" };
            profileToSave.Users.Add(new User(testUser, "password123"));
            profileToSave.GrantPrivilege("UsersTable", Privilege.Select);
            profileToSave.GrantPrivilege("UsersTable", Privilege.Insert);
            managerToSave.Profiles.Add(profileToSave);

            string path = Path.Combine(Directory.GetCurrentDirectory(), dbName);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            managerToSave.Save(dbName);

            try
            {
                Manager result = Manager.Load(dbName, testUser);

                Assert.NotNull(result);

                Profile profile = result.ProfileByName("AdminProfile");
                Assert.NotNull(profile);
                Assert.Equal("AdminProfile", profile.Name);

                Assert.True(profile.IsGrantedPrivilege("UsersTable", Privilege.Select));
                Assert.True(profile.IsGrantedPrivilege("UsersTable", Privilege.Insert));
                Assert.False(profile.IsGrantedPrivilege("UsersTable", Privilege.Delete));
            }
            finally
            {
                if (Directory.Exists(path)) Directory.Delete(path, true);
            }
        }


        [Fact]
        public void TestIsPassCorrTrue()
        {
            Assert.True(manager.IsPasswordCorrect("admin","pass123"));
        }

        [Fact]
        public void TestIsPassCorrFalse()
        {
            Assert.False(manager.IsPasswordCorrect("admin","wrongpassword"));
        }

        [Fact]
        public void TestIsUserAdmin()
        {
            User admin = new User("admin", "1234");

            Profile adminProfile = new Profile();
            adminProfile.Name = Profile.AdminProfileName;
            adminProfile.Users.Add(admin);

            Manager managerAdmin = new Manager("admin");
            managerAdmin.Profiles.Add(adminProfile);

            Assert.True(managerAdmin.IsUserAdmin());
        }

        [Fact]
        public void TestUsrByNameCorrect()
        {
            User fUser = manager.UserByName("admin");
            Assert.NotNull(fUser);
            Assert.Equal("admin",fUser.Username);
        }

        [Fact]
        public void TestUsrByNameNoCorrect()
        {
            User fUser = manager.UserByName("unknownUser");
            Assert.Null(fUser);
        }

        [Fact]
        public void TestProfrByNameCorrect()
        {
            Profile fProfile = manager.ProfileByName("UserPrf");
            Assert.NotNull(fProfile);
            Assert.Equal("UserPrf",fProfile.Name);
        }

        [Fact]
        public void TestProfrByNameNoCorrect()
        {
            Profile fProfile = manager.ProfileByName("NonExistentProfile");            
            Assert.Null(fProfile);
        
            User user = new User("user", "1234");

            Profile normalProfile = new Profile();
            normalProfile.Name = "Users";
            normalProfile.Users.Add(user);

            Manager managerUser = new Manager("user");
            managerUser.Profiles.Add(normalProfile);

            Assert.False(managerUser.IsUserAdmin());
        }





        [Fact]
        public void TestIsGrantedPrivilege()
        {
            Manager emptyManager = new Manager("admin");

            Assert.False(emptyManager.IsGrantedPrivilege(null, "Users", Privilege.Select));
            Assert.False(emptyManager.IsGrantedPrivilege("user", null, Privilege.Select));
            Assert.False(emptyManager.IsGrantedPrivilege("unknown", "Users", Privilege.Select));

            User admin = new User("admin", "1234");

            Profile adminProfile = new Profile();
            adminProfile.Name = Profile.AdminProfileName;
            adminProfile.Users.Add(admin);

            Manager adminManager = new Manager("admin");
            adminManager.Profiles.Add(adminProfile);

            Assert.True(adminManager.IsGrantedPrivilege("admin", "AnyTable", Privilege.Select));
            Assert.True(adminManager.IsGrantedPrivilege("admin", "AnyTable", Privilege.Insert));
            Assert.True(adminManager.IsGrantedPrivilege("admin", "AnyTable", Privilege.Update));
            Assert.True(adminManager.IsGrantedPrivilege("admin", "AnyTable", Privilege.Delete));

            User user = new User("user", "1234");

            Profile userProfile = new Profile();
            userProfile.Name = "Players";
            userProfile.Users.Add(user);
            userProfile.GrantPrivilege("Users", Privilege.Select);

            Manager normalManager = new Manager("user");
            normalManager.Profiles.Add(userProfile);

            Assert.True(normalManager.IsGrantedPrivilege("user", "Users", Privilege.Select));
            Assert.False(normalManager.IsGrantedPrivilege("user", "Users", Privilege.Delete));
            Assert.False(normalManager.IsGrantedPrivilege("user", "NoTable", Privilege.Select));
        }

        [Fact]
        public void TestAddProfile()
        {
            User admin = new User("admin", "1234");

            Profile adminProfile = new Profile();
            adminProfile.Name = Profile.AdminProfileName;
            adminProfile.Users.Add(admin);

            Manager adminManager = new Manager("admin");
            adminManager.Profiles.Add(adminProfile);

            Profile newProfile = new Profile();
            newProfile.Name = "Players";

            adminManager.AddProfile(newProfile);

            Assert.NotNull(adminManager.ProfileByName("Players"));
            Assert.Equal(2, adminManager.Profiles.Count);

            int countBeforeNull = adminManager.Profiles.Count;
            adminManager.AddProfile(null);
            Assert.Equal(countBeforeNull, adminManager.Profiles.Count);

            Profile duplicateProfile = new Profile();
            duplicateProfile.Name = "Players";

            adminManager.AddProfile(duplicateProfile);

            Assert.Equal(1, adminManager.Profiles.Count(p => p.Name == "Players"));

            User user = new User("user", "1234");

            Profile normalProfile = new Profile();
            normalProfile.Name = "Users";
            normalProfile.Users.Add(user);

            Manager userManager = new Manager("user");
            userManager.Profiles.Add(normalProfile);

            Profile anotherProfile = new Profile();
            anotherProfile.Name = "AnotherProfile";

            userManager.AddProfile(anotherProfile);

            Assert.Null(userManager.ProfileByName("AnotherProfile"));
            Assert.Single(userManager.Profiles);
        }

        [Fact]
        public void TestRemoveProfile()
        {
            User admin = new User("admin", "1234");

            Profile adminProfile = new Profile();
            adminProfile.Name = Profile.AdminProfileName;
            adminProfile.Users.Add(admin);

            Profile playersProfile = new Profile();
            playersProfile.Name = "Players";

            Manager adminManager = new Manager("admin");
            adminManager.Profiles.Add(adminProfile);
            adminManager.Profiles.Add(playersProfile);

            Assert.True(adminManager.RemoveProfile("Players"));
            Assert.Null(adminManager.ProfileByName("Players"));

            Assert.False(adminManager.RemoveProfile("NoProfile"));

            User user = new User("user", "1234");

            Profile normalProfile = new Profile();
            normalProfile.Name = "Users";
            normalProfile.Users.Add(user);

            Profile testProfile = new Profile();
            testProfile.Name = "TestProfile";

            Manager userManager = new Manager("user");
            userManager.Profiles.Add(normalProfile);
            userManager.Profiles.Add(testProfile);

            Assert.False(userManager.RemoveProfile("TestProfile"));
            Assert.NotNull(userManager.ProfileByName("TestProfile"));
        }

    }
}



