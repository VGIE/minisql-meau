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
    public class ProfileTests
    {
        [Fact]
        public void TestGrantPrivilege()
        {
            Profile profile = new Profile();

            bool result = profile.GrantPrivilege("Users", Privilege.Select);

            Assert.True(result);
            Assert.True(profile.IsGrantedPrivilege("Users", Privilege.Select));
        }

        [Fact]
        public void TestRevokePrivilege()
        {
            Profile profile = new Profile();
            profile.GrantPrivilege("Users", Privilege.Select);

            bool result = profile.RevokePrivilege("Users", Privilege.Select);

            Assert.True(result);
            Assert.False(profile.IsGrantedPrivilege("Users", Privilege.Select));
        }

        [Fact]
        public void TestIsGrantedPrivilege()
        {
            Profile profile = new Profile();
            profile.GrantPrivilege("Users", Privilege.Select);

            bool result = profile.IsGrantedPrivilege("Users", Privilege.Select);

            Assert.True(result);
        }




        [Fact]
        public void TestGrantPrivilegeDuplicate()
        {
            Profile profile = new Profile();
            profile.GrantPrivilege("Users", Privilege.Select);

            bool result = profile.GrantPrivilege("Users", Privilege.Select);

            Assert.False(result);
        }
        [Fact]
        public void TestGrantPrivilegeNullTable()
        {
            Profile profile = new Profile();

            bool result = profile.GrantPrivilege(null, Privilege.Select);

            Assert.False(result);
        }
        [Fact]
        public void TestRevokePrivilegeNonExisting()
        {
            Profile profile = new Profile();

            bool result = profile.RevokePrivilege("Users", Privilege.Select);

            Assert.False(result);
        }
        [Fact]
        public void TestIsGrantedPrivilegeNonExisting()
        {
            Profile profile = new Profile();
            profile.GrantPrivilege("Users", Privilege.Select);

            bool result = profile.IsGrantedPrivilege("Users", Privilege.Delete);

            Assert.False(result);
        }
    }
}