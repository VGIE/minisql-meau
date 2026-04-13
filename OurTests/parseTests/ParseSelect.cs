using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using DbManager;
using Xunit;

namespace OurTests
{
    public class ParseSelect
    {
        //TODO DEADLINE 1A : Create your own tests for Table
        private Database db;
        private List<ColumnDefinition> dColumns;
        private List <string>values;
        private Table table;
        private MiniSQLParser minSQLParser;
        
        public ParseSelect()
        {
            db= new Database("admin","1234");
            minSQLParser = new MiniSQLParser();

            dColumns = new List<ColumnDefinition>
            {
                new ColumnDefinition(ColumnDefinition.DataType.Int,"Age"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Double,"Height")
            };

            table = new Table("TestTable", dColumns);

            values = new List<string> { "20", "Unai", "1.76" };
            table.AddRow(new Row(dColumns, values));
            values = new List<string> { "21", "Maialen", "1.50" };
            table.AddRow(new Row(dColumns, values));
            values = new List<string> { "22", "Endika", "1.80" };
            table.AddRow(new Row(dColumns, values));

            db.AddTable(table);
        }
       
       
        
        [Fact]
        public void TestSelAllC()
        {
            Select select = MiniSQLParser.Parse("SELECT Name,Height,Age FROM TestTable") as Select;
            Assert.Equal("TestTable", select.Table);
            Assert.Contains("Name", select.Columns);
            Assert.Contains("Height", select.Columns);
            Assert.Contains("Age", select.Columns);
            Assert.Null(select.Where);
    
        }

        [Fact]
        public void TestSelWithoutC()
        {
            var select = MiniSQLParser.Parse("SELECT FROM TestTable");           
            Assert.Null(select);
        }

        [Fact]
        public void TestSelConNoMatch()
        {
            var select = MiniSQLParser.Parse("SELECT Name,Age FROM TestTable WHERE Age>'100'");

            Assert.Null(select);
        }

        [Fact]
        public void TestSelAllCWithAS()
        {
            Select select = MiniSQLParser.Parse("SELECT * FROM TestTable") as Select;
            Assert.Contains("*",select.Columns);
            
        }


        [Fact]
        public void TestSelWithCon()
        {
            var select = MiniSQLParser.Parse("SELECT Name,Age FROM TestTable WHERE Age>'50'");
            Assert.Null(select);
        }

        [Fact]
        public void TestSelelectColumnsNotOrderWithCon()
        {
            var select = MiniSQLParser.Parse("SELECT Age,Name FROM TestTable WHERE Name='Rodolfo'");
            Assert.Null(select);
        }


        [Fact]
        public void TestSelWithDifferentOp()
        {
           
            var select1 = MiniSQLParser.Parse("SELECT Name,Age FROM TestTable WHERE Age='25'");
            Assert.Null(select1);

            var select2 = MiniSQLParser.Parse("SELECT Name,Height FROM TestTable WHERE Height<'1.60'");
            Assert.Null(select2);

            var select3 = MiniSQLParser.Parse("SELECT Name,Age FROM TestTable WHERE Name='Maider'");
            Assert.Null(select3);

           
        }
        //comentario para poder hacer commit

        



    }
}