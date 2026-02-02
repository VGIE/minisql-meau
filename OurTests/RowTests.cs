using DbManager;

namespace OurTests
{
    public class RowTests
    {
        //TODO DEADLINE 1A : Create your own tests for Row
        
        [Fact]
        public void TestIsTrue()
        {

        }

        [Fact]
        public void TestAsText()
        {
            var columns = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "C1"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "C2"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "C3"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "C4"),
            };

            var values = new List<string> { "A", "B:C", "D:", "E" };

            var row = new Row(columns, values);

            var result = row.AsText();

            Assert.Equal("A:B[SEPARATOR]C:D[SEPARATOR]:E",result);
        }
        
    }
}