using System;
using System.Linq;
using NBi.Core.ResultSet.Comparer;

namespace NBi.Core.ResultSet
{
    public class Column : IColumnDefinition
    {
        public int Index {get; set;} 
        public ColumnRole Role {get; set;} 
        public ColumnType Type {get; set;}

        public bool IsToleranceSpecified
        {
            get { return !string.IsNullOrEmpty(Tolerance); }
        }

        public string Tolerance { get; set; }

        public Rounding.RoundingStyle RoundingStyle { get; set; }
        public string RoundingStep { get; set; }
    }
}
