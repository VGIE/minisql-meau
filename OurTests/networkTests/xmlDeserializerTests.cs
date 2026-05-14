using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbManager.Network;

namespace OurTests.networkTests
{
    public class xmlDeserializerTests
    {
        [Fact]
        public void ParseCreateTest()
        {
            string command = "<Create Database='MyDb' User='admin' Password='password123' />";
            bool result = XmlDeserializer.ParseCreate(command, out string db, out string user, out string pass);
            Assert.True(result);
            Assert.Equal("MyDb", db);
            Assert.Equal("admin", user);
            Assert.Equal("password123", pass);
        }

        [Fact]
        public void ParseQueryTest()
        {
            string command = "<Query>SELECT * FROM Users</Query>";
            bool result = XmlDeserializer.ParseQuery(command, out string query);
            Assert.True(result);
            Assert.Equal(string.Empty, query);
        }

        [Fact]
        public void ParseOpenTest()
        {
            string command = "<Open Database=\"MyDb\" User=\"admin\" Password=\"password123\"/>";

            bool result = XmlDeserializer.ParseOpen(command, out string db, out string user, out string pass);

            Assert.True(result);
            Assert.Equal("MyDb", db);
            Assert.Equal("admin", user);
            Assert.Equal("password123", pass);
        }
        [Fact]
        public void ParseQueryAnswerTest()
        {
            string goodAnswer = "<Result>Some content</Result>";
            bool result1 = XmlDeserializer.ParseQueryAnswer(goodAnswer, out string answerContent1);
            Assert.True(result1);
            Assert.Equal(goodAnswer, answerContent1);
            string errorAnswer = "<Error>Database not found</Error>";
            bool result2= XmlDeserializer.ParseQueryAnswer(errorAnswer, out string answerContent2);
            Assert.False(result2);
            Assert.Equal("Database not found", answerContent2);
        }
    }
}
