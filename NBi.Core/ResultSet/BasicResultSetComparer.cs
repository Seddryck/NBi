using System;
using System.Collections;
using System.Data;

namespace NBi.Core.ResultSet
{
    public class BasicResultSetComparer: IResultSetComparer, IComparer
    {
        public BasicResultSetComparer()
        {
        }

        public int Compare(Object x, Object y)
        {
            return this.Compare(x, y);
        }

        int IComparer.Compare(Object x, Object y)
        {
            if (x == null)
                throw new ArgumentNullException("x cannot be null");

            if (y == null)
                throw new ArgumentNullException("y cannot be null");

            if (x is DataSet && y is DataSet)
                return StringComparer.InvariantCultureIgnoreCase.Compare(BuildResultSet((DataSet)y).RawValue, BuildResultSet((DataSet)x).RawValue);

            if (x is ResultSet && y is DataSet)
                return StringComparer.InvariantCultureIgnoreCase.Compare(BuildResultSet((DataSet)y).RawValue, ((ResultSet)x).RawValue);

            if (x is DataSet && y is ResultSet)
                return StringComparer.InvariantCultureIgnoreCase.Compare(((ResultSet)y).RawValue, BuildResultSet((DataSet)x).RawValue);

            if (x is ResultSet && y is ResultSet)
                return StringComparer.InvariantCultureIgnoreCase.Compare(((ResultSet)y).RawValue, ((ResultSet)x).RawValue);

            throw new ArgumentException(string.Format("first argument is of type '{0}' and second '{1}'. There is no implementention of comparaison between these two types.",x.GetType().ToString(), y.GetType().ToString()));
        }

        protected internal virtual ResultSet BuildResultSet(DataSet ds)
        {
            var csvWriter = new ResultSetCsvWriter("");
            var rs = csvWriter.BuildContent(ds.Tables[0]);
            return new ResultSet(rs);
        }       
    }
}
