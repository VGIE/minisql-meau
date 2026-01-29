using DbManager;

namespace OurTests
{
    public class ColumnDefinitionsTests
    {
        //TODO DEADLINE 1A : Create your own tests for Table
        /*
        [Fact]
        public void Test1()
        {

        }
        */

        [Fact]
        public void TestEncode()
        {

            var column = new ColumnDefinition();

            var result = column.Encode("->");
            
            Assert.Equal("->", "[ARROW]");
        }
    }
}