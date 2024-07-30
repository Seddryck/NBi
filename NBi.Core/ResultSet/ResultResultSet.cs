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
        public IEnumerable<IResultRow>? Missing { get; set; }
        public IEnumerable<IResultRow>? Unexpected { get; set; }
        public IEnumerable<IResultRow>? Duplicated { get; set; }
        public Sample? NonMatchingValue { get; set; }


        public static ResultResultSet Matching
        {
            get => new() { Difference = ResultSetDifferenceType.None };
        }

        public static ResultResultSet NotMatching
        {
            get => new() { Difference = ResultSetDifferenceType.Content };
        }

        public static ResultResultSet StructureNotComparable
        {
            get => new() {Difference= ResultSetDifferenceType.Structure};
        }

        public static ResultResultSet Build(IEnumerable<IResultRow> missingRows, IEnumerable<IResultRow> unexpectedRows, IEnumerable<IResultRow> duplicatedRows, IEnumerable<IResultRow> keyMatchingRows, IEnumerable<IResultRow> nonMatchingValueRows)
        {
            var res = !missingRows.Any() && !unexpectedRows.Any() && !duplicatedRows.Any() && !nonMatchingValueRows.Any()
                    ? new ResultResultSet() { Difference = ResultSetDifferenceType.None }
                    : new ResultResultSet() { Difference = ResultSetDifferenceType.Content };

            res.Missing=missingRows;
            res.Unexpected = unexpectedRows;
            res.Duplicated = duplicatedRows;
            res.NonMatchingValue = GetSubset(nonMatchingValueRows, keyMatchingRows);

            return res;
        }

        private static Sample GetSubset(IEnumerable<IResultRow> rows, IEnumerable<IResultRow> reference)
            =>  new (rows?.ToList() ?? [], reference, rows?.Count() ?? 0);

        public class Sample(IEnumerable<IResultRow> rows, IEnumerable<IResultRow> refs, int count)
        {
            public IEnumerable<IResultRow> Rows { get; set; } = rows;
            public IEnumerable<IResultRow> References { get; set; } = refs;
            public int Count { get; set; } = count;
        }

        public override bool Equals(object? obj)
            => obj is ResultResultSet && obj.GetHashCode() == GetHashCode();

        public override int GetHashCode()
            => Difference.GetHashCode();

        public override string ToString()
            => Difference.ToString();
    }
}
