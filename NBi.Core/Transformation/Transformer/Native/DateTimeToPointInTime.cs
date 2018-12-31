using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    abstract class DateTimeToPointInTime : INativeTransformation
    {

        public object Evaluate(object value)
        {
            if (value == null)
                return null;
            else if (value is DateTime)
                return EvaluateDateTime((DateTime)value);
            else
                throw new NotImplementedException();
        }

        protected abstract object EvaluateDateTime(DateTime value);
    }

    class DateTimeToFirstOfMonth : DateTimeToPointInTime
    {
        protected override object EvaluateDateTime(DateTime value) => new DateTime(value.Year, value.Month, 1);
    }

    class DateTimeToFirstOfYear : DateTimeToPointInTime
    {
        protected override object EvaluateDateTime(DateTime value) => new DateTime(value.Year, 1, 1);
    }

    class DateTimeToLastOfMonth : DateTimeToPointInTime
    {
        protected override object EvaluateDateTime(DateTime value) => new DateTime(value.Year, value.Month, 1).AddMonths(1).AddDays(-1);
    }

    class DateTimeToLastOfYear : DateTimeToPointInTime
    {
        protected override object EvaluateDateTime(DateTime value) => new DateTime(value.Year, 12, 31);
    }
}
