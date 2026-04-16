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
        [Fact]
        public void TestRevokePrivilege()
        {
            Manager manager = new Manager("Admin");
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
            string securityContent = "admin,1234,AdminProfile,UsersTable,Select/Insert";

            string path = Path.Combine(Directory.GetCurrentDirectory(), dbName);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string filePath = Path.Combine(path, "security.dat");
            File.WriteAllText(filePath, securityContent);

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
    }
}