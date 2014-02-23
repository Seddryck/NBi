using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet
{
    public enum ResultSetRowDifferenceType
    {
        Key,
        Value
    }
    
    public class ResultSetDifference
    {
        public int RowIndex { get; set; }
        public ResultSetRowDifferenceType DiffType { get; set; }
    }
}
