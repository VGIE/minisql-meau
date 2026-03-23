using System.Collections.Generic;
using Xunit;
using DbManager;

namespace ParsingTests
{
    public class UpdateTests
    {
        [Fact]
        public void CorrectWithSpaces()
        {
            var query = MiniSQLParser.Parse("UPDATE Table SET Col1='Val1', Col2=10 WHERE Col3 = 'Val3'") as DbManager.Update;
            Assert.NotNull(query);
            Assert.Equal("Table", query.Table);
            Assert.Equal(2, query.Columns.Count);
            
            var query2 = MiniSQLParser.Parse("UPDATE    Table   SET    Col1='Val1'  ,   Col2=10   WHERE    Col3 = 'Val3'") as DbManager.Update;
            Assert.NotNull(query2);
            Assert.Equal("Table", query2.Table);
        }

        [Fact]
        public void StringValue()
        {
            var query = MiniSQLParser.Parse("UPDATE Table SET Col1 = 'Val1' WHERE Col3 = 'Val3'") as DbManager.Update;
            Assert.NotNull(query);
            Assert.Equal("Table", query.Table);
            Assert.Equal("Col1", query.Columns[0].ColumnName);
        }

        [Fact]
        public void IntValues()
        {
            var query = MiniSQLParser.Parse("UPDATE Table SET Col1 = 10, Col2 = 20 WHERE Col3 = 30") as DbManager.Update;
            Assert.NotNull(query);
        }

        [Fact]
        public void IntValue()
        {
            var query = MiniSQLParser.Parse("UPDATE Table SET Col1 = 10 WHERE Col3 = 30") as DbManager.Update;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectSpacesOrMissingApostrophes()
        {
            var query = MiniSQLParser.Parse("UPDATE Table SET Col1 = Val1 WHERE Col3 = Val3");
            Assert.Null(query);
            var query2 = MiniSQLParser.Parse("UPDATE Table SET Col1='Val1'WHERE Col3='Val3'");
            Assert.Null(query2); // Requires spaces
        }

        [Fact]
        public void StringValues()
        {
            var query = MiniSQLParser.Parse("UPDATE Table SET Col1 = 'Val1', Col2 = 'Val2' WHERE Col3 = 'Val3'") as DbManager.Update;
            Assert.NotNull(query);
        }

        [Fact]
        public void DoubleValue()
        {
            var query = MiniSQLParser.Parse("UPDATE Table SET Col1 = 10.5 WHERE Col3 = 30.5") as DbManager.Update;
            Assert.NotNull(query);
        }

        [Fact]
        public void DoubleValues()
        {
            var query = MiniSQLParser.Parse("UPDATE Table SET Col1 = 10.5, Col2 = 20.5 WHERE Col3 = 30.5") as DbManager.Update;
            Assert.NotNull(query);
        }
    }

    public class CreateTableTests
    {
        [Fact]
        public void IncorrectCapitalizationOrWithoutTableOrColumns()
        {
            Assert.Null(MiniSQLParser.Parse("Create TABLE Tab1 (Col1 INT)"));
            Assert.Null(MiniSQLParser.Parse("CREATE TABLE (Col1 INT)"));
            Assert.Null(MiniSQLParser.Parse("CREATE TABLE Tab1 ()"));
        }

        [Fact]
        public void SimpleOneColumnTableWithSpaces()
        {
            var query = MiniSQLParser.Parse("CREATE    TABLE   Tab1   (   Col1    INT   )") as DbManager.CreateTable;
            Assert.NotNull(query);
        }

        [Fact]
        public void SimpleOneColumnTable()
        {
            var query = MiniSQLParser.Parse("CREATE TABLE Tab1 (Col1 INT)") as DbManager.CreateTable;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectWithMultipleColumnsAndSpaces()
        {
            var query = MiniSQLParser.Parse("CREATE TABLE Tab1 (Col1 INT ,)");
            Assert.Null(query);
        }

        [Fact]
        public void CorrectWithMultipleColumns()
        {
            var query = MiniSQLParser.Parse("CREATE TABLE Tab1 (Col1 INT, Col2 STRING)") as DbManager.CreateTable;
            Assert.NotNull(query);
            Assert.Equal(2, query.ColumnsParameters.Count);
        }
    }

    public class SelectTests
    {
        [Fact]
        public void SelectWithSingleColumnMultipleSpaces()
        {
            var query = MiniSQLParser.Parse("SELECT   Col1     FROM    Table") as DbManager.Select;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectSelectWithTextAfter()
        {
            Assert.Null(MiniSQLParser.Parse("SELECT Col1 FROM Table WHERE Col1 = 1 TEXTAFTER"));
        }

        [Fact]
        public void IncorrectSelectWithMultipleColumnsAndSpacesBetweenColumns()
        {
            var query = MiniSQLParser.Parse("SELECT Col1 , , Col2 FROM Table");
            Assert.Null(query);
        }
    }

    public class DropTableTests
    {
        [Fact]
        public void Correct()
        {
            var query = MiniSQLParser.Parse("DROP TABLE Tab1") as DbManager.DropTable;
            Assert.NotNull(query);
        }

        [Fact]
        public void CorrectWithSpaces()
        {
            var query = MiniSQLParser.Parse("DROP   TABLE    Tab1") as DbManager.DropTable;
            Assert.NotNull(query);
        }
    }

    public class InsertTests
    {
        [Fact]
        public void StringValues()
        {
            var query = MiniSQLParser.Parse("INSERT INTO Tab1 VALUES ('Val1', 'Val2')") as DbManager.Insert;
            Assert.NotNull(query);
        }

        [Fact]
        public void SimpleIntValues()
        {
            var query = MiniSQLParser.Parse("INSERT INTO Tab1 VALUES (10, 20)") as DbManager.Insert;
            Assert.NotNull(query);
        }

        [Fact]
        public void SimpleStringValues()
        {
            var query = MiniSQLParser.Parse("INSERT INTO Tab1 VALUES ('Val1', 'Val2')") as DbManager.Insert;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectSpacesOrMissingCommas()
        {
            Assert.Null(MiniSQLParser.Parse("INSERT INTO Tab1 VALUES (10 20)"));
        }

        [Fact]
        public void SimpleDoubleValues()
        {
            var query = MiniSQLParser.Parse("INSERT INTO Tab1 VALUES (10.5, 20.5)") as DbManager.Insert;
            Assert.NotNull(query);
        }

        [Fact]
        public void CorrectWithSpaces()
        {
            var query = MiniSQLParser.Parse("INSERT   INTO    Tab1   VALUES   (  'Val1'  ,  'Val2'  )") as DbManager.Insert;
            Assert.NotNull(query);
        }
    }

    public class DeleteTests
    {
        [Fact]
        public void IncorrectSpaces()
        {
            Assert.Null(MiniSQLParser.Parse("DELETEFROM Table WHERE Col1=10"));
        }

        [Fact]
        public void SimpleDoubleCondition()
        {
            var query = MiniSQLParser.Parse("DELETE FROM Table WHERE Col1 = 10.5") as DbManager.Parser.Delete;
            Assert.NotNull(query);
        }

        [Fact]
        public void SimpleIntCondition()
        {
            var query = MiniSQLParser.Parse("DELETE FROM Table WHERE Col1 = 10") as DbManager.Parser.Delete;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectCapitalizationOrMissingCommasInLiterals()
        {
            Assert.Null(MiniSQLParser.Parse("Delete FROM Table WHERE Col1 = 10"));
        }

        [Fact]
        public void SimpleStringCondition()
        {
            var query = MiniSQLParser.Parse("DELETE FROM Table WHERE Col1 = 'Val1'") as DbManager.Parser.Delete;
            Assert.NotNull(query);
        }

        [Fact]
        public void CorrectWithSpaces()
        {
            var query = MiniSQLParser.Parse("DELETE   FROM    Table    WHERE   Col1  =  10") as DbManager.Parser.Delete;
            Assert.NotNull(query);
        }
    }
}
