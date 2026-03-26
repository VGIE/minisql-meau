using System;
using System.ComponentModel.DataAnnotations.Schema;
using DbManager;
using NuGet.Frameworks;
using Xunit;

namespace OurTests.parseTests
{
    public class ParseUpdateTests
    {
        private Database db;
        private List<ColumnDefinition> definitionColumn;
        [Fact]
        public void ParseUpdateTest()
        {
            db = new Database("admin", "1234");
            definitionColumn = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Height")
            };
            Table table = new Table("Users", definitionColumn);
            var rowValues = new List<String> {"Ana", "20", "1.65" };
            table.AddRow(new Row(definitionColumn, rowValues));
            db.AddTable(table);
        }
        [Fact]
        public void CorrectWithSpaces()
        {
            var query = MiniSQLParser.Parse("UPDATE Table SET Col1='string1', Col2=10 WHERE Col3 = 'string3'");
            Assert.Null(query);
            var query2 = MiniSQLParser.Parse("UPDATE  Table SET Col1='string1', Col2=10 WHERE Col3 = 'string3'");
            Assert.Null(query2);
        }
        [Fact]
        public void StringValue()
        {
            var query = MiniSQLParser.Parse("UPDATE Table SET Col1='string1' WHERE Col3='string3'") as Update;
            Assert.NotNull(query);
            Assert.Equal("Table", query.Table);
        }
        [Fact]
        public void IntValue()
        {
            var query = MiniSQLParser.Parse("UPDATE Table SET Col1=10 WHERE Col3=30") as Update;
            Assert.NotNull(query);
        }
        [Fact]
        public void DoubleValue()
        {
            var query = MiniSQLParser.Parse("UPDATE Table SET Col1=10.5 WHERE Col3=30.5") as Update;
            Assert.NotNull(query);
        }
        [Fact]
        public void IncorrectSpacesOrMissingApostrophes()
        {
            Assert.Null(MiniSQLParser.Parse("UPDATE Table SET Col1=Val1WHERE Col3=Val3"));
            Assert.Null(MiniSQLParser.Parse("UPDATE Table SET Col1='Val1'WHERE Col3='Val3'"));
        }
        [Fact]
        public void IncorrectWithoutSET()
        {
            Assert.Null(MiniSQLParser.Parse("UPDATE Table Col1='Val1"));
        }
        [Fact]
        public void IncorrectCapitalization()
        {
            Assert.Null(MiniSQLParser.Parse("update Table SET Col1='Val1'"));
        }
    }
}
