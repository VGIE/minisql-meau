using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using DbManager;
using Xunit;

namespace OurTests
{
    public class testDropTable
    {
        //TODO DEADLINE 1A : Create your own tests for Table
        private Database db;
        private List<ColumnDefinition> dColumns;
        private List <string>values;
        private Table table;
        private List<Table> Tables = new List<Table>();

        
        public testDropTable()
        {
            db= new Database("admin","1234");

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
        //public int NumTables()
        //{
            //return Tables.Count;
        //}
       
        
        [Fact]
        public void TestDropT()
        {
            Assert.Equal(1, db.NumTables());

        var dropTable = MiniSQLParser.Parse("DROP TABLE Peronas");
        string dropped = dropTable.Execute(db);

        Assert.Equal(Constants.TableDoesNotExistError, dropped);
    
        }
[Fact]
    public void TestDropTableTrue()
    {
        var dropTable = MiniSQLParser.Parse("DROP TABLE TestTable");
        string dropped = dropTable.Execute(db);

        Assert.Equal(Constants.DropTableSuccess, dropped);
        Assert.Equal(0, db.NumTables());
    }
        
        

        



    }
}