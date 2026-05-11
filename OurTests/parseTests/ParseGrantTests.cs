using DbManager;
using DbManager.Parser;
using DbManager.Security;
using NuGet.Frameworks;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Xunit;

namespace OurTests.parseTests
{
    public class ParseGrantTests
    {
        [Fact]
        public void ValidQuery()
        {
            var query = MiniSQLParser.Parse("GRANT SELECT ON Users TO admin") as Grant;
            Assert.NotNull(query);
            Assert.Equal("SELECT", query.PrivilegeName);
            Assert.Equal("Users", query.TableName);
            Assert.Equal("admin", query.ProfileName);
        }

        [Fact]
        public void ValidQueryWithAnotherPrivilege()
        {
            var query = MiniSQLParser.Parse("GRANT INSERT ON Products TO manager") as Grant;
            Assert.NotNull (query);
            Assert.Equal("INSERT", query.PrivilegeName);
            Assert.Equal("Products", query.TableName);
            Assert.Equal("manager", query.ProfileName);
        }
        [Fact]
        public void IncorrectCapitalization()
        {
            Assert.Null(MiniSQLParser.Parse("Grant SELECT ON Users TO admin"));
        }
        [Fact]
        public void IncorrectWithoutOn()
        {
            Assert.Null(MiniSQLParser.Parse("GRANT SELECT Users TO admin"));
        }
        [Fact]
        public void IncorrectWithoutTo()
        {
            Assert.Null(MiniSQLParser.Parse("GRANT SELECT ON Users admin"));
        }
        [Fact]
        public void IncorrectWithoutPrivilege()
        {
            Assert.Null(MiniSQLParser.Parse("GRANT ON Users TO admin"));
        }
        [Fact]
        public void IncorrectWithoutTable()
        {
            Assert.Null(MiniSQLParser.Parse("GRANT SELECT ON TO admin"));
        }
    }
}
