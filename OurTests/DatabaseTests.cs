using DbManager;
using System.Collections.Generic;

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

    
}