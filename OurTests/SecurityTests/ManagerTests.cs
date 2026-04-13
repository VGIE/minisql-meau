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
        public void TestIsUserAdmin()
        {
            User admin = new User("admin", "1234");

            Profile adminProfile = new Profile();
            adminProfile.Name = Profile.AdminProfileName;
            adminProfile.Users.Add(admin);

            Manager managerAdmin = new Manager("admin");
            managerAdmin.Profiles.Add(adminProfile);

            Assert.True(managerAdmin.IsUserAdmin());

            User user = new User("user", "1234");

            Profile normalProfile = new Profile();
            normalProfile.Name = "Users";
            normalProfile.Users.Add(user);

            Manager managerUser = new Manager("user");
            managerUser.Profiles.Add(normalProfile);

            Assert.False(managerUser.IsUserAdmin());
        }

       
    }
}



