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
        }
    }
}