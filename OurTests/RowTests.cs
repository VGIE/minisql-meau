using DbManager;

namespace OurTests
{
    public class RowTests
    {
        //TODO DEADLINE 1A : Create your own tests for Row
        /*
        [Fact]
        public void Test1()
        {

        }
        */
        [Fact]
        public void GetValueAndSetValueTest()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age"),
            };
            List<string> rowValues = new List<string>()
            {
                "Jacinto", "37"
            };
            Row testRow = new Row(columns, rowValues);
            Assert.Equal("Jacinto", testRow.GetValue("Name"));
            Assert.Equal("37", testRow.GetValue("Age"));

            testRow.SetValue("Name", "Maider");
            Assert.Equal("Maider", testRow.GetValue("Name"));
            Assert.Null(testRow.GetValue("Nombre"));
        }
    }
}