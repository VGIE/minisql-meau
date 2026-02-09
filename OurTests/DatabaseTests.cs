using DbManager;

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
    }

    
}