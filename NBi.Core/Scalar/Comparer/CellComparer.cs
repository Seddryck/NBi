using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Comparer
{
    public class CellComparer
    {
        private readonly NumericComparer numericComparer = new NumericComparer();
        private readonly TextComparer textComparer = new TextComparer();
        private readonly DateTimeComparer dateTimeComparer = new DateTimeComparer();
        private readonly BooleanComparer booleanComparer = new BooleanComparer();

        public ComparerResult Compare(object x, object y, ColumnType columnType, Tolerance? tolerance, Rounding? rounding)
        {
            //Any management
            if (x.ToString() != "(any)" && y.ToString() != "(any)")
            {
                //Null management
                if (DBNull.Value.Equals(x))
                {
                    if (!DBNull.Value.Equals(y) && y.ToString() != "(null)" && y.ToString() != "(blank)")
                        return new ComparerResult("(null)");
                }
                else if (DBNull.Value.Equals(y))
                {
                    if (!DBNull.Value.Equals(x) && x.ToString() != "(null)" && x.ToString() != "(blank)")
                        return new ComparerResult(x.ToString() ?? string.Empty);
                }
                //(value) management
                else if (x.ToString() == "(value)" || y.ToString() == "(value)")
                {
                    if (DBNull.Value.Equals(x) || DBNull.Value.Equals(y))
                        return new ComparerResult(DBNull.Value.Equals(y) ? "(null)" : x.ToString() ?? string.Empty);
                }
                //Not Null management
                else
                {
                    //Numeric
                    if (columnType == ColumnType.Numeric)
                    {
                        //Convert to decimal
                        if (rounding != null)
                            return numericComparer.Compare(x, y, rounding);
                        else
                            return numericComparer.Compare(x, y, tolerance ?? NumericAbsoluteTolerance.None);
                    }
                    //Date and Time
                    else if (columnType == ColumnType.DateTime)
                    {
                        //Convert to dateTime
                        if (rounding != null)
                            return dateTimeComparer.Compare(x, y, rounding);
                        else if (tolerance!=null && tolerance!=DateTimeTolerance.None)
                            return dateTimeComparer.Compare(x, y, tolerance);
                        else
                            return dateTimeComparer.Compare(x, y);
                    }
                    //Boolean
                    else if (columnType == ColumnType.Boolean)
                    {
                        //Convert to bool
                        return booleanComparer.Compare(x, y);
                    }
                    //Text
                    else
                    {
                        if (tolerance == null || tolerance==TextSingleMethodTolerance.None)
                            return textComparer.Compare(x, y);
                        else
                            return textComparer.Compare(x, y, tolerance);
                    }
                }
            }
            return ComparerResult.Equality;
        }
    }
}
