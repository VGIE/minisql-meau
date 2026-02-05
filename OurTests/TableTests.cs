using DbManager;

namespace OurTests
{
    public class TableTests
    {
        [Fact]
        public void ColumnIndexByNameTest()
        {
            Table table = Table.CreateTestTable();

            int indexName = table.ColumnIndexByName("Name");
            int indexAge = table.ColumnIndexByName("Age");
            int indexWrong = table.ColumnIndexByName("NonExistent");

            Assert.Equal(0, indexName); 
            Assert.Equal(2, indexAge);  
            Assert.Equal(-1, indexWrong);
        }

        [Fact]
        public void TestGetColumn()
        {

        }
    }
}