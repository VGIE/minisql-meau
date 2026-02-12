using DbManager;
using System.Collections.Generic;
using DbManager.Parser;
using System.Numerics;
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

        [Fact]
        public void TestDeleteWhere()
        {
            Database db = Database.CreateTestDatabase();

            Condition greaterThan20 = new("Age", ">", "20");
            bool resultOk = db.DeleteWhere(Table.TestTableName, greaterThan20);

            Assert.True(resultOk);
            Assert.Equal(0, db.TableByName(Table.TestTableName).NumRows());

            Condition condCualquiera = new("Name", "=", "Rodolfo");
            bool tableNotFound = db.DeleteWhere("TablaImaginaria", condCualquiera);

            Assert.False(tableNotFound);
            Assert.Equal(Constants.TableDoesNotExistError, db.LastErrorMessage);

            db = Database.CreateTestDatabase();
            Condition condColInexistente = new("Peso", ">", "70");
            bool colNotFound = db.DeleteWhere(Table.TestTableName, condColInexistente);

            Assert.False(colNotFound);
            Assert.Equal(Constants.ColumnDoesNotExistError, db.LastErrorMessage);

            db = Database.CreateTestDatabase();
            Condition condUno = new("Name", "=", "Maider");
            db.DeleteWhere(Table.TestTableName, condUno);

            Assert.Equal(2, db.TableByName(Table.TestTableName).NumRows());
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

        [Fact]
        public void TestAddTable()
        {
            Database db = Database.CreateTestDatabase();

            List<ColumnDefinition> cols = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Product"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Price")
            };
            Table newTable = new Table("Products", cols);

            bool resultOk = db.AddTable(newTable);

            Assert.True(resultOk);
            Assert.Equal(Constants.CreateTableSuccess, db.LastErrorMessage);
            Assert.NotNull(db.TableByName("Products"));

           
            Table tableDuplicate = new Table(Table.TestTableName, cols);
            bool resultDuplicate = db.AddTable(tableDuplicate);

            Assert.False(resultDuplicate);
            Assert.Equal(Constants.TableAlreadyExistsError, db.LastErrorMessage);
        }



    }
}
