using DbManager;
using System.Collections.Generic;
using DbManager.Parser;
namespace OurTests
{
    public class UnitTest1
    {
        //TODO DEADLINE 1B : Create your own tests for Database
        /*
        [Fact]
        public void Test1()
        {

        }
        */

        /*DropTable - Maialen*/

        [Fact]
        public void DropTableEX()
        {
            Database testDataBase = Database.CreateTestDatabase();
            string tableName = "TestTable";

            Assert.NotNull(testDataBase.TableByName(tableName));
            Assert.True(testDataBase.DropTable(tableName));
            Assert.Equal(Constants.DropTableSuccess, testDataBase.LastErrorMessage);
            Assert.Null(testDataBase.TableByName(tableName));
        }

        /*InsertTable - Maialen*/

        [Fact]
        public void TestInsertTrue()
        {
            Database testDatabase = Database.CreateTestDatabase();
            string tableName = "TestTable";
            List<string> row = new List<string> { "Maialen", "21", "1.54" };
            Assert.True(testDatabase.Insert(tableName, row));
            Assert.Equal(Constants.InsertSuccess, testDatabase.LastErrorMessage);
            List<List<string>> expectedRows = new List<List<string>> { row };
            testDatabase.CheckForTesting(tableName, expectedRows);
        }

        [Fact]
        public void TestInsertMultipleRows()
        {
            Database testDatabase = Database.CreateTestDatabase();
            string tableName = "TestTable";
            List<List<string>> newRows = new List<List<string>>
    {
        new List<string> { "Unai", "21", "1.78" },
        new List<string> { "Endika", "21", "1.80" }
    };
            testDatabase.AddTuplesForTesting(tableName, newRows);
            Table t = testDatabase.TableByName(tableName);
            Assert.True(t.NumRows() >= 2);
        }
    }
}