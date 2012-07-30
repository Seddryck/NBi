using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NBi.Core.ResultSet
{
    public enum ResultSetDifferenceType
    {
        None,
        Structure,
        Content,
    }

    public class ResultSetCompareResult
    {
        public bool Value { get; set; }
        public ResultSetDifferenceType Difference { get; set; }

        public static ResultSetCompareResult Matching
        {
            get
            {
                return new ResultSetCompareResult() { Difference = ResultSetDifferenceType.None };
            }
        }

        public static ResultSetCompareResult NotMatching
        {
            get
            {
                return new ResultSetCompareResult() { Difference = ResultSetDifferenceType.Content };
            }
        }

        public static ResultSetCompareResult StructureNotComparable
        {
            get
            {
                return new ResultSetCompareResult() {Difference= ResultSetDifferenceType.Structure};
            }
        }

        public static ResultSetCompareResult Build(IEnumerable<DataRow> missingRows, IEnumerable<DataRow> unexpectedRows, IEnumerable<DataRow> keyMatchingRows, IEnumerable<DataRow> nonMatchingValueRows)
        {
            if (missingRows.Count() == 0 && unexpectedRows.Count() == 0 && nonMatchingValueRows.Count() == 0)
                return new ResultSetCompareResult() { Difference = ResultSetDifferenceType.None };
            else
                return new ResultSetCompareResult() { Difference = ResultSetDifferenceType.Content };
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ResultSetCompareResult))
                return false;
            else
                return obj.GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.Difference.GetHashCode();
        }

        public override string ToString()
        {
            return Difference.ToString();
        }
    }
}
