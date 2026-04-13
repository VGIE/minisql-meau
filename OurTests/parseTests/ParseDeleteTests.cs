using System;
using System.ComponentModel.DataAnnotations.Schema;
using DbManager;
using NuGet.Frameworks;
using Xunit;

namespace OurTests.parseTests
{
    public class ParseDeleteTests
    {
        private Database db;
        private List<ColumnDefinition> definitionColumn;
        [Fact]
        public void ParseDelete()
        {
            db = new Database("admin", "1234");
            definitionColumn = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Height")
            };
            Table table = new Table("Users", definitionColumn);
            var rowValues = new List<String> { "Ana", "20", "1.65" };
            table.AddRow(new Row(definitionColumn, rowValues));
            db.AddTable(table);
        }
        [Fact]
        public void CorrectWithSpaces()
        {
            var query = MiniSQLParser.Parse("DELETE  FROM Table WHERE Col1 = '10'");
            Assert.Null(query);
        }
        [Fact]
        public void SimpleStringCondition()
        {
            var query = MiniSQLParser.Parse("DELETE FROM Table WHERE Col1='Val1'") as DbManager.Parser.Delete;
            Assert.NotNull(query);
        }
        [Fact]
        public void SimpleIntCondition()
        {
            var query = MiniSQLParser.Parse("DELETE FROM Table WHERE Col1='10'") as DbManager.Parser.Delete;
            Assert.NotNull (query);
        }
        [Fact]
        public void SimpleDoubleCondition()
        {
            var query = MiniSQLParser.Parse("DELETE FROM Table WHERE Col1='10.5'") as DbManager.Parser.Delete;
            Assert.NotNull(query);
        }
        [Fact]
        public void IncorrectSpaces()
        {
            Assert.Null(MiniSQLParser.Parse("DELETEFROM Table WHERE Col1 ='10'"));
        }
        [Fact]
        public void IncorrectCapitalization()
        {
            Assert.Null(MiniSQLParser.Parse("Delete FROM Table WHERE Col1 = '10'"));

        }
    }
}
