using DbManager;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Xunit;

namespace OurTests
{
    public class RowTests
    {
        //TODO DEADLINE 1A : Create your own tests for Row
        private Row CreateTestRow()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age"),
            };


            List<string> rowvalues = new List<string>
            {
                "Eduardo", "67"
            };
            Row testRow = new Row(columns, rowvalues);
            return testRow;
        }


        [Fact]
        public void TestIsTrue()
        {
            Row testRow = CreateTestRow();

            Condition nameCond = new Condition("Name", "=", "Eduardo");
            Assert.True(testRow.IsTrue(nameCond));

            Condition nameCondWrong = new Condition("Name", "=", "Maider");
            Assert.False(testRow.IsTrue(nameCondWrong));

            Condition ageCond = new Condition("Age", ">", "18");
            Assert.True(testRow.IsTrue(ageCond));

            Condition ageCondYounger = new Condition("Age", "<", "30");
            Assert.False(testRow.IsTrue(ageCondYounger));

            Condition ageExact = new Condition("Age", "=", "67");
            Assert.True(testRow.IsTrue(ageExact));
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