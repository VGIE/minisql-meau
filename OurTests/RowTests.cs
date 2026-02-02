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
            var row = new Row();
            row.Values = new List<string> { "A", "B->C", "D->", "E" };

            var result = row.AsText();

            Assert.Equal("A->B[ARROW]C->D[ARROW]->E", result);
        }
        
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