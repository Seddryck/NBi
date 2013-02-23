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
        public Sample Missing { get; set; }
        public Sample Unexpected { get; set; }
        public Sample Duplicated { get; set; }
        public Sample NonMatchingValue { get; set; }


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

        public static ResultSetCompareResult Build(IEnumerable<DataRow> missingRows, IEnumerable<DataRow> unexpectedRows, IEnumerable<DataRow> duplicatedRows, IEnumerable<DataRow> keyMatchingRows, IEnumerable<DataRow> nonMatchingValueRows)
        {
            ResultSetCompareResult res = null;

            if (missingRows.Count() == 0 && unexpectedRows.Count() == 0 && duplicatedRows.Count()==0 && nonMatchingValueRows.Count() == 0)
                res = new ResultSetCompareResult() { Difference = ResultSetDifferenceType.None };
            else
                res = new ResultSetCompareResult() { Difference = ResultSetDifferenceType.Content };

            res.Missing=GetSubset(missingRows);
            res.Unexpected = GetSubset(unexpectedRows);
            res.Duplicated = GetSubset(duplicatedRows);
            res.NonMatchingValue = GetSubset(nonMatchingValueRows, keyMatchingRows);

            return res;
        }

        private const int MAX_ROWS_RESULT = 10;
        private const int COUNT_ROWS_SAMPLE_RESULT = 10;

        private static Sample GetSubset(IEnumerable<DataRow> rows)
        {
            var subset = new List<DataRow>(MAX_ROWS_RESULT);
            
            if (rows.Count() > MAX_ROWS_RESULT)
                subset = rows.Take(COUNT_ROWS_SAMPLE_RESULT).ToList();
            else
                subset = rows.ToList();

            return new Sample(subset, null, rows.Count());
        }

        private static Sample GetSubset(IEnumerable<DataRow> rows, IEnumerable<DataRow> reference)
        {
            var subset = new List<DataRow>(MAX_ROWS_RESULT);

            if (rows.Count() > MAX_ROWS_RESULT)
                subset = rows.Take(COUNT_ROWS_SAMPLE_RESULT).ToList();
            else
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
