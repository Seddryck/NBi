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

    public class ResultResultSet
    {
        public bool Value { get; set; }
        public ResultSetDifferenceType Difference { get; set; }
        public IEnumerable<DataRow> Missing { get; set; }
        public IEnumerable<DataRow> Unexpected { get; set; }
        public IEnumerable<DataRow> Duplicated { get; set; }
        public Sample NonMatchingValue { get; set; }


        public static ResultResultSet Matching
        {
            get
            {
                return new ResultResultSet() { Difference = ResultSetDifferenceType.None };
            }
        }

        public static ResultResultSet NotMatching
        {
            get
            {
                return new ResultResultSet() { Difference = ResultSetDifferenceType.Content };
            }
        }

        public static ResultResultSet StructureNotComparable
        {
            get
            {
                return new ResultResultSet() {Difference= ResultSetDifferenceType.Structure};
            }
        }

        public static ResultResultSet Build(IEnumerable<DataRow> missingRows, IEnumerable<DataRow> unexpectedRows, IEnumerable<DataRow> duplicatedRows, IEnumerable<DataRow> keyMatchingRows, IEnumerable<DataRow> nonMatchingValueRows)
        {
            ResultResultSet res = null;

            if (missingRows.Count() == 0 && unexpectedRows.Count() == 0 && duplicatedRows.Count()==0 && nonMatchingValueRows.Count() == 0)
                res = new ResultResultSet() { Difference = ResultSetDifferenceType.None };
            else
                res = new ResultResultSet() { Difference = ResultSetDifferenceType.Content };

            res.Missing=missingRows;
            res.Unexpected = unexpectedRows;
            res.Duplicated = duplicatedRows;
            res.NonMatchingValue = GetSubset(nonMatchingValueRows, keyMatchingRows);

            return res;
        }


        private static Sample GetSubset(IEnumerable<DataRow> rows, IEnumerable<DataRow> reference)
        {
            var subset = new List<DataRow>(rows.Count());
            subset = rows.ToList();
            return new Sample(subset, reference, rows.Count());
        }

        public class Sample
        {
            public IEnumerable<DataRow> Rows { get; set; }
            public IEnumerable<DataRow> References { get; set; }
            public int Count { get; set; }

            public Sample(IEnumerable<DataRow> rows, IEnumerable<DataRow> refs, int count)
            {
                Rows = rows;
                References = refs;
                Count = count;
            }

        }

        public override bool Equals(object obj)
        {
            if (!(obj is ResultResultSet))
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
