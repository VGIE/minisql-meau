using Xunit;
using DbManager;
using System.Collections.Generic;

namespace OurTests
{
    public class RowTests
    {
        //TODO DEADLINE 1A : Create your own tests for Row
        
        [Fact]
        public void TestIsTrue()
        {
            List<ColumnDefinition> ColumnDefinitions = new List<ColumnDefinition>();
        }

        [Fact]
        public void TestAsText()
        {
            var row = new Row();
            row.Values = new List<string> { "A", "B->C", "D->", "E" };

            var result = row.AsText();

            Assert.Equal("A->B[ARROW]C->D[ARROW]->E", result);
        }
        
    }
}