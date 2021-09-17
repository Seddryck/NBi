using NBi.Extensibility;
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
        public IEnumerable<IResultRow> Missing { get; set; }
        public IEnumerable<IResultRow> Unexpected { get; set; }
        public IEnumerable<IResultRow> Duplicated { get; set; }
        public Sample NonMatchingValue { get; set; }


        public static ResultResultSet Matching
        {
            get => new ResultResultSet() { Difference = ResultSetDifferenceType.None };
        }

        public static ResultResultSet NotMatching
        {
            get => new ResultResultSet() { Difference = ResultSetDifferenceType.Content };
        }

        public static ResultResultSet StructureNotComparable
        {
            get => new ResultResultSet() {Difference= ResultSetDifferenceType.Structure};
        }

        public static ResultResultSet Build(IEnumerable<IResultRow> missingRows, IEnumerable<IResultRow> unexpectedRows, IEnumerable<IResultRow> duplicatedRows, IEnumerable<IResultRow> keyMatchingRows, IEnumerable<IResultRow> nonMatchingValueRows)
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


        private static Sample GetSubset(IEnumerable<IResultRow> rows, IEnumerable<IResultRow> reference)
            =>  new Sample(rows?.ToList() ?? new List<IResultRow>(0), reference, rows?.Count() ?? 0);

        public class Sample
        {
            public IEnumerable<IResultRow> Rows { get; set; }
            public IEnumerable<IResultRow> References { get; set; }
            public int Count { get; set; }

            public Sample(IEnumerable<IResultRow> rows, IEnumerable<IResultRow> refs, int count)
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
