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
        
        [Fact]
        public void TestGetColumnAndNumColumns()
        {
            Table table = Table.CreateTestTable();

            int index = 2;

            var column = table.GetColumn(index);

            Assert.NotNull(column);

            int numCols = table.NumColumns();

            Assert.Equal(3, numCols);

        }

        [Fact]
        public void TestColumnByName()
        {
            Table table = Table.CreateTestTable();

            var colName = table.ColumnByName("Name");
            var colAge = table.ColumnByName("Age");
            var colWrong = table.ColumnByName("NonExistent");

            Assert.NotNull(colName);
            Assert.Equal("Name", colName.Name);

            Assert.NotNull(colAge);
            Assert.Equal("Age", colAge.Name);

            Assert.Null(colWrong);
        }

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
        public void ToStringTest()
        {
            Table table = Table.CreateTestTable();

            string expected = "['Name','Height','Age']{'Rodolfo','1.62','25'}{'Maider','1.67','67'}{'Pepe','1.55','51'}";
            Assert.Equal(expected, table.ToString());

            

            Table tableEmpty = new Table("Empty", new List<ColumnDefinition>());
            Assert.Equal("", tableEmpty.ToString());

            List<ColumnDefinition> columns = new List<ColumnDefinition>
             {
                new ColumnDefinition(ColumnDefinition.DataType.String, "OnlyColumns")
            };

            Table tableOnlyColumns = new Table("OnlyCols", columns);
            Assert.Equal("['OnlyColumns']", tableOnlyColumns.ToString());
        }
        [Fact]
        public void InsertTest()
        {
            Table table = Table.CreateTestTable();
            Assert.True(table.Insert(new List<string> { "Aitana", "1.70", "20" }));
            Assert.Equal(4, table.NumRows());
            Assert.Equal("Aitana", table.GetRow(3).Values[0]);


            Assert.False(table.Insert(new List<string> { "Error" }));
            Assert.Equal(4, table.NumRows());
        }

        [Fact]
        public void TestDeleteIthRow()
        {
            Table table = Table.CreateTestTable();
            int rowCount = table.NumRows();
            int indexToDelete = 1;

            string expectedName = Table.TestColumn1Row3;

            table.DeleteIthRow(indexToDelete);

            Assert.Equal(rowCount - 1, table.NumRows());
            Assert.Equal(expectedName, table.GetRow(indexToDelete).Values[0]);
            Assert.Equal(Table.TestColumn1Row1, table.GetRow(0).Values[0]);
        }                                            

    }
