using DbManager;

namespace OurTests
{
    public class TableTests
    {
        //TODO DEADLINE 1A : Create your own tests for Table
        
        [Fact]
        public void TestGetRow()
        {
            List<ColumnDefinition>columns = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.String,"Name")
            };
            Table table = new Table("TestTable",columns);
            Row row1 = new Row(columns, new List<string> { "Maialen" });
            Row row2 = new Row(columns, new List<string> { "Unai" });
            table.AddRow(row1);
            table.AddRow(row2);
            Assert.NotNull(table.GetRow(0));
            Assert.Equal("Maialen", table.GetRow(0).Values[0]);
            Assert.NotNull(table.GetRow(1));
            Assert.Equal("Unai", table.GetRow(1).Values[0]);
            Assert.Null(table.GetRow(2));
            Assert.Null(table.GetRow(-1));
        }
        [Fact]
        public void AddRowAndNumRows()
        {
          List<ColumnDefinition> columns = new List<ColumnDefinition> 
            { 
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name") 
            };
            Table table = new Table("TestTable", columns);
            Assert.Equal(0, table.NumRows());  
            table.AddRow(new Row(columns, new List<string> { "Maialen" }));
            Assert.Equal(1, table.NumRows());
            table.AddRow(new Row(columns, new List<string> { "Unai" }));
            Assert.Equal(2, table.NumRows());
        }
        
    }
}