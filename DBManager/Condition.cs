using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;
using DbManager;

namespace DbManager
{
    public class Condition
    {
        public string ColumnName { get; private set; }
        public string Operator { get; private set; }
        public string LiteralValue { get; private set; }

        public Condition(string column, string op, string literalValue)  // Maialen
        {
            //TODO DEADLINE 1A: Initialize member variables
            ColumnName = column;
            Operator = op;
            LiteralValue = literalValue;
        }


        public bool IsTrue(string value, ColumnDefinition.DataType type) // Maialen
        {
            //TODO DEADLINE 1A: return true if the condition is true for this value
            //Depending on the type of the column, the comparison should be different:
            //"ab" < "cd
            //"9" > "10"
            //9 < 10
            //Convert first the strings to the appropriate type and then compare (depending on the operator of the condition)

            int comparar;
            if (type ==ColumnDefinition.DataType.Int)
            {
                int val = int.Parse(value);
                int limit = int.Parse(LiteralValue);
                comparar=val.CompareTo(limit);
            }else if (type==ColumnDefinition.DataType.Double)
            {
                double val = double.Parse(value);
                double limit = double.Parse(LiteralValue);
                comparar=val.CompareTo(limit);
            }
            else
            {
                comparar = string.Compare(value, LiteralValue);
            }

            if (Operator == ">") return comparar > 0;
            if (Operator == "<") return comparar < 0;
            if (Operator == "=" || Operator == "==") return comparar == 0;
            if (Operator == "!=") return comparar != 0;
            
            return false;
            
        }
    }
}