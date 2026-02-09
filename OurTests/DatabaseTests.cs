using DbManager;
using System.Collections.Generic;
using DbManager.Parser;
namespace OurTests
{
    public class DatabaseTests
    {
        //TODO DEADLINE 1B : Create your own tests for Database
        /*
        [Fact]
        public void Test1()
        {

        }
        */
        [Fact]
        public void TestTableByName()
        {
            Database db = Database.CreateTestDatabase();
            string nameToSearch = Table.TestTableName;

            Table table = db.TableByName(nameToSearch);

            Assert.NotNull(table);
            Assert.Equal(nameToSearch, table.Name);

            Table ghostTable = db.TableByName("TablaInexistente");
            Assert.Null(ghostTable);
        }

        [Fact]
        public void TestCreateTable()
        {
            Database db = Database.CreateTestDatabase();

            List<ColumnDefinition> newCols =
            [
                new ColumnDefinition(ColumnDefinition.DataType.String, "Street")
            ];

            List<ColumnDefinition> noCols = [];

            bool tableAlreadyExists = db.CreateTable("TestTable", newCols);
            bool tableHasNoCols = db.CreateTable("TestTableNoCols", noCols);
            bool tableCreated = db.CreateTable("PerfectTestTable", newCols);

            Assert.False(tableAlreadyExists);
            Assert.False(tableHasNoCols);
            Assert.True(tableCreated);

            Assert.NotNull(db.TableByName("PerfectTestTable"));
            Assert.Equal("Street", db.TableByName("PerfectTestTable").GetColumn(0).Name);
        }
    }




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
            string name = Table.TestTableName;
            List<string> rowMaialen = new List<string> { "Maialen", "1.54", "21" };
            bool result = testDatabase.Insert(name, rowMaialen);
            Assert.True(result);
            Assert.Equal(Constants.InsertSuccess, testDatabase.LastErrorMessage);
            Table t = testDatabase.TableByName(name);
            Assert.Equal(4, t.NumRows());
            List<List<string>> expectedRows = new List<List<string>>
            {
                new List<string> { "Endika", "1.80", "21" },
                new List<string> { "Unai", "1.78", "21" },
                new List<string> { "Aitana", "1.62", "21" },
                rowMaialen
            };

            testDatabase.CheckForTesting(name, expectedRows);

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
