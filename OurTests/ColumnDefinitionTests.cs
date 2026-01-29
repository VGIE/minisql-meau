using DbManager;

namespace OurTests
{
    public class ColumnDefinitionsTests
    {
        //TODO DEADLINE 1A : Create your own tests for Table
        
        [Fact]
        public void Test1()
        {

        }
        */
        [Fact]
        public void TestAsText()
        {
            var stringColumn = new ColumnDefinition(ColumnDefinition.DataType.String, "Name");
            Assert.Equal("Name->String", stringColumn.AsText());

            var intColumn = new ColumnDefinition(ColumnDefinition.DataType.Int, "Age");
            Assert.Equal("Age->Int", intColumn.AsText());

            var doubleColumn = new ColumnDefinition(ColumnDefinition.DataType.Double, "Price");
            Assert.Equal("Price->Double", doubleColumn.AsText());
        }

        [Fact]
        public void TestParse()
        {
            Assert.Equal("Name", ColumnDefinition.Parse("Name->String").Name);
            Assert.Equal("Age", ColumnDefinition.Parse("Age->Int").Name);
            Assert.Equal("Price", ColumnDefinition.Parse("Price->Double").Name);
            Assert.Null(ColumnDefinition.Parse("invalid")); //igual sobra
        }
    }
}