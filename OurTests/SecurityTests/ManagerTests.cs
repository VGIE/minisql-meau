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
    }
}