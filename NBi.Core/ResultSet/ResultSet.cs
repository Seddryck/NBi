using System.Collections.Generic;

namespace NBi.Core.ResultSet
{
    public class ResultSet
    {
        public readonly string RawValue;
        public readonly IList<ResultSetRow> Rows;

        public ResultSet(string rawValue)
        {
            RawValue = rawValue;
        }

        public ResultSet(IList<ResultSetRow> rows)
        {
            Rows = rows;
        }

        public bool EqualTo(ResultSet other)
        {
            if (this.Rows.Count != other.Rows.Count)
                return false;

            int i = 0;
            IEnumerator<ResultSetRow> iterX = this.Rows.GetEnumerator();
            IEnumerator<ResultSetRow> iterY = other.Rows.GetEnumerator();

            while (iterX.MoveNext() && iterY.MoveNext())
            {
                if (iterX.Current is ResultSetRow)
                    //if (!(iterX.Current).EquivalentTo(iterY.Current))
                        return false;
            }
            return true;
        }

    }
}
