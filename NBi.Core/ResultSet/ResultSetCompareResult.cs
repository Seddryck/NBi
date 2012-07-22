using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet
{
    public enum ResultSetDifferenceType
    {
        Structure,
        Content,
    }

    public class ResultSetCompareResult
    {
        public bool Value { get; set; }
        public ResultSetDifferenceType Difference { get; set; }

        public static ResultSetCompareResult StructureNotComparable
        {
            get
            {
                return new ResultSetCompareResult() {Difference= ResultSetDifferenceType.Structure};
            }
        }
    }
}
