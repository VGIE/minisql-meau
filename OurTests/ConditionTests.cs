using DbManager;

namespace OurTests
{
    public class ConditionTests
    {
        //TODO DEADLINE 1A : Create your own tests for Condition
        
        [Fact]
        public void TestIntMayor()
        {
            Condition con = new Condition("Edad",">","2");
            bool resultTrue = con.IsTrue("3",ColumnDefinition.DataType.Int);
            bool resultFalse = con.IsTrue("1",ColumnDefinition.DataType.Int);

            Assert.True(resultTrue);
            Assert.False(resultFalse);

        }

        [Fact]
        public void TestIntMenor()
        {
           Condition con = new Condition("Edad","<","2");
            bool resultTrue = con.IsTrue("0",ColumnDefinition.DataType.Int);
            bool resultFalse = con.IsTrue("10",ColumnDefinition.DataType.Int);

            Assert.True(resultTrue);
            Assert.False(resultFalse);

        }

         [Fact]
        public void TestDouble()
        {
            Condition con = new Condition("Precio","=","2.2");
            bool resultTrue = con.IsTrue("2.2",ColumnDefinition.DataType.Double);
            bool resultFalse = con.IsTrue("2.75",ColumnDefinition.DataType.Double);

            Assert.True(resultTrue);
            Assert.False(resultFalse);

        }

        [Fact]
        public void TestString()
        {
            Condition conMayor = new Condition("Nombre",">","c");
            Assert.True(conMayor.IsTrue("d", ColumnDefinition.DataType.String));
            Assert.False(conMayor.IsTrue("a", ColumnDefinition.DataType.String));

            Condition conMenor = new Condition("Codigo", "<", "cd");
            Assert.True(conMenor.IsTrue("ab", ColumnDefinition.DataType.String));
            Assert.False(conMenor.IsTrue("xy", ColumnDefinition.DataType.String)); 
        }

        [Fact]
        public void TestStringIgual()
        {
            Condition conIgual = new Condition("Ciudad","=","Bilbao");
            Assert.True(conIgual.IsTrue("Bilbao", ColumnDefinition.DataType.String));
            Assert.False(conIgual.IsTrue("Vitoria", ColumnDefinition.DataType.String));

        }
        
    }
}