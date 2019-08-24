using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    abstract class AbstractDateTimeTransformation : INativeTransformation
    {

        public object Evaluate(object value)
        {
            switch (value)
            {
                case null: return EvaluateNull();
                case DBNull dbnull: return EvaluateNull();
                case DateTime dt: return EvaluateDateTime(dt);
                default: return EvaluateUncasted(value);
            }
        }

        private object EvaluateUncasted(object value)
        {
            if (value as string == "null")
                EvaluateNull();

            var caster = new DateTimeCaster();
            var dateTime = caster.Execute(value);
            return EvaluateDateTime(dateTime);
        }

        protected virtual object EvaluateNull() => null;
        protected abstract object EvaluateDateTime(DateTime value);
    }

    class DateTimeToDate : AbstractDateTimeTransformation
    {
        protected override object EvaluateDateTime(DateTime value) => value.Date;
    }

    class DateToAge : AbstractDateTimeTransformation
    {
        protected override object EvaluateNull() => 0;
        protected override object EvaluateDateTime(DateTime value)
        {
            // Save today's date.
            var today = DateTime.Today;
            // Calculate the age.
            var age = today.Year - value.Year;
            // Go back to the year the person was born in case of a leap year
            return value.AddYears(age) > today ? age-- : age;
        }
    }

    class DateTimeToFirstOfMonth : AbstractDateTimeTransformation
    {
        protected override object EvaluateDateTime(DateTime value) => new DateTime(value.Year, value.Month, 1);
    }

    class DateTimeToFirstOfYear : AbstractDateTimeTransformation
    {
        protected override object EvaluateDateTime(DateTime value) => new DateTime(value.Year, 1, 1);
    }

    class DateTimeToLastOfMonth : AbstractDateTimeTransformation
    {
        protected override object EvaluateDateTime(DateTime value) => new DateTime(value.Year, value.Month, 1).AddMonths(1).AddDays(-1);
    }

    class DateTimeToLastOfYear : AbstractDateTimeTransformation
    {
        protected override object EvaluateDateTime(DateTime value) => new DateTime(value.Year, 12, 31);
    }

    class DateTimeToNextDay : AbstractDateTimeTransformation
    {
        protected override object EvaluateDateTime(DateTime value) => value.AddDays(1);
    }

    class DateTimeToNextMonth : AbstractDateTimeTransformation
    {
        protected override object EvaluateDateTime(DateTime value) => value.AddMonths(1);
    }

    class DateTimeToNextYear : AbstractDateTimeTransformation
    {
        protected override object EvaluateDateTime(DateTime value) => value.AddYears(1);
    }

    class DateTimeToPreviousDay : AbstractDateTimeTransformation
    {
        protected override object EvaluateDateTime(DateTime value) => value.AddDays(-1);
    }

    class DateTimeToPreviousMonth : AbstractDateTimeTransformation
    {
        protected override object EvaluateDateTime(DateTime value) => value.AddMonths(-1);
    }

    class DateTimeToPreviousYear : AbstractDateTimeTransformation
    {
        protected override object EvaluateDateTime(DateTime value) => value.AddYears(-1);
    }

    class DateTimeToClip : AbstractDateTimeTransformation
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

    class DateTimeToSetTime : AbstractDateTimeTransformation
    {
        public TimeSpan Instant { get; }

        public DateTimeToSetTime(string instant)
        {
            Instant = TimeSpan.Parse(instant);
        }

        protected override object EvaluateDateTime(DateTime value)
            => new DateTime(value.Year, value.Month, value.Day, Instant.Hours, Instant.Minutes, Instant.Seconds);
    }

    class NullToDate : AbstractDateTimeTransformation
    {
        public DateTime Default { get; }

        public NullToDate(string dt)
        {
            var caster = new DateTimeCaster();
            Default = caster.Execute(dt);
        }

        protected override object EvaluateNull() => Default;
        protected override object EvaluateDateTime(DateTime value) => value;
    }

    class DateTimeToFloorHour : AbstractDateTimeTransformation
    {
        protected override object EvaluateDateTime(DateTime value) 
            => value.AddTicks(-1 * (value.Ticks % TimeSpan.TicksPerHour));
    }

    class DateTimeToCeilingHour : AbstractDateTimeTransformation
    {
        protected override object EvaluateDateTime(DateTime value)
            => value.AddTicks(TimeSpan.TicksPerHour - (value.Ticks % TimeSpan.TicksPerHour == 0 ? TimeSpan.TicksPerHour : value.Ticks % TimeSpan.TicksPerHour));
    }

    class DateTimeToFloorMinute : AbstractDateTimeTransformation
    {
        protected override object EvaluateDateTime(DateTime value)
            => value.AddTicks(-1 * (value.Ticks % TimeSpan.TicksPerMinute));
    }

    class DateTimeToCeilingMinute : AbstractDateTimeTransformation
    {
        protected override object EvaluateDateTime(DateTime value)
            => value.AddTicks(TimeSpan.TicksPerMinute - (value.Ticks % TimeSpan.TicksPerMinute == 0 ? TimeSpan.TicksPerMinute : value.Ticks % TimeSpan.TicksPerMinute));
    }
}
