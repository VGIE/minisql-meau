using DbManager.Parser;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DbManager
{
    public class Row
    {
        private List<ColumnDefinition> ColumnDefinitions = new List<ColumnDefinition>();
        public List<string> Values { get; set; }

        public Row(List<ColumnDefinition> columnDefinitions, List<string> values) // Endika
        {
            //TODO DEADLINE 1.A: Initialize member variables
            this.ColumnDefinitions = columnDefinitions;
            this.Values = values;
        }

        public void SetValue(string columnName, string value) // Endika
        {
            //TODO DEADLINE 1.A: Given a column name and value, change the value in that column
            for (int i = 0; i < this.ColumnDefinitions.Count; i++)
            {
                if (ColumnDefinitions[i].Name == columnName)
                {
                    Values[i]=value;
                    return;
                }
            }
        }

        public string GetValue(string columnName) // Endika
        {
            //TODO DEADLINE 1.A: Given a column name, return the value in that column
            for (int i = 0; i < this.ColumnDefinitions.Count; i++)
            {
                if (ColumnDefinitions[i].Name == columnName) {
                return Values[i];
                }
            }
            return null;
        }

        public bool IsTrue(Condition condition) // Aitana
        {
            //TODO DEADLINE 1.A: Given a condition (column name, operator and literal value, return whether it is true or not
            //for this row. Check Condition.IsTrue method
           

            ColumnDefinition c = null;

            foreach (var col in ColumnDefinitions)
            {
                if (col.Name == condition.ColumnName)
                {
                    c = col;
                }
            }
            if (c == null)
            {
                return false;
            }
            string rowValue = GetValue(condition.ColumnName);
           
            return condition.IsTrue(rowValue, c.Type);

        }

        private const string Delimiter = ":";
        private const string DelimiterEncoded = "[SEPARATOR]";

        private static string Encode(string value) // Maialen
        {
            //TODO DEADLINE 1.C: Encode the delimiter in value

            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            return value.Replace(Delimiter,DelimiterEncoded);
            
            
        }

        private static string Decode(string value) // Maialen
        {
            //TODO DEADLINE 1.C: Decode the value doing the opposite of Encode()
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            return value.Replace(DelimiterEncoded,Delimiter);
            
        }

        public string AsText() // Unai
        {
            //TODO DEADLINE 1.C: Return the row as string with all values separated by the delimiter
            List<string> newValues = new();

            foreach(string v in Values)
            {
                string encodedValue = Encode(v);
                newValues.Add(encodedValue);

            }

            return string.Join(Delimiter, newValues);
            
        }

        public static Row Parse(List<ColumnDefinition> columns, string value) // Unai
        {
            //TODO DEADLINE 1.C: Parse a rowReturn the row as string with all values separated by the delimiter
            string[] parts = value.Split(Delimiter);

            Row row = new(columns, []);

            foreach (string part in parts)
            {
                string decoded = Decode(part);
                row.Values.Add(decoded);
            }
            
            return row;
            
        }
    }
}
