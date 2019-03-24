using NBi.Core.Scalar.Casting;
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
            switch (value)
            {
                case null: return null;
                case DateTime dt: return EvaluateDateTime(dt);
                default:
                    var caster = new DateTimeCaster();
                    var dateTime = caster.Execute(value);
                    return EvaluateDateTime(dateTime);
            }
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

    class DateTimeToNextDay : DateTimeToPointInTime
    {
        protected override object EvaluateDateTime(DateTime value) => value.AddDays(1);
    }

    class DateTimeToNextMonth : DateTimeToPointInTime
    {
        protected override object EvaluateDateTime(DateTime value) => value.AddMonths(1);
    }

    class DateTimeToNextYear : DateTimeToPointInTime
    {
        protected override object EvaluateDateTime(DateTime value) => value.AddYears(1);
    }

    class DateTimeToPreviousDay : DateTimeToPointInTime
    {
        protected override object EvaluateDateTime(DateTime value) => value.AddDays(-1);
    }

    class DateTimeToPreviousMonth : DateTimeToPointInTime
    {
        protected override object EvaluateDateTime(DateTime value) => value.AddMonths(-1);
    }

    class DateTimeToPreviousYear : DateTimeToPointInTime
    {
        protected override object EvaluateDateTime(DateTime value) => value.AddYears(-1);
    }

    class DateTimeToClip : DateTimeToPointInTime
    {
        public DateTime Min { get; }
        public DateTime Max { get; }

        public DateTimeToClip(string min, string max)
        {
            var caster = new DateTimeCaster();
            Min = caster.Execute(min);
            Max = caster.Execute(max);
        }

        protected override object EvaluateDateTime(DateTime value) => (value < Min) ? Min : (value > Max) ? Max : value;
    }
}
