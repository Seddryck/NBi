using NBi.Core.Scalar.Casting;
using NBi.Core.Scalar.Resolver;
using NBi.Extensibility;
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
                case DBNull _: return EvaluateNull();
                case DateTime dt: return EvaluateDateTime(dt);
                default: return EvaluateUncasted(value);
            }
        }

        private object EvaluateUncasted(object value)
        {
            if (value as string == "(null)" || (value is string && string.IsNullOrEmpty(value as string)))
                return EvaluateNull();

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
        public IScalarResolver<DateTime> Min { get; }
        public IScalarResolver<DateTime> Max { get; }

        public DateTimeToClip(IScalarResolver<DateTime> min, IScalarResolver<DateTime> max)
            => (Min, Max) = (min, max);

        protected override object EvaluateDateTime(DateTime value)
            => (value < Min.Execute()) ? Min.Execute() : (value > Max.Execute()) ? Max.Execute() : value;
    }

    class DateTimeToSetTime : AbstractDateTimeTransformation
    {
        public IScalarResolver<string> Instant { get; }

        public DateTimeToSetTime(IScalarResolver<string> instant)
            => Instant = instant;

        protected override object EvaluateDateTime(DateTime value)
        {
            var time = TimeSpan.Parse(Instant.Execute());
            return new DateTime(value.Year, value.Month, value.Day, time.Hours, time.Minutes, time.Seconds);
        }
    }

    class NullToDate : AbstractDateTimeTransformation
    {
        public IScalarResolver<DateTime> Default { get; }

        public NullToDate(IScalarResolver<DateTime> dt)
            => Default = dt;

        protected override object EvaluateNull() => Default.Execute();
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

    class DateTimeToAdd : AbstractDateTimeTransformation
    {
        public IScalarResolver<int> Times { get; }
        public IScalarResolver<string> TimeSpan { get; }

        public DateTimeToAdd(IScalarResolver<string> timeSpan, IScalarResolver<int> times)
            => (TimeSpan, Times) = (timeSpan, times);

        public DateTimeToAdd(IScalarResolver<string> timeSpan)
            : this(timeSpan, new LiteralScalarResolver<int>(1)) { }

        protected override object EvaluateDateTime(DateTime value)
            => value.AddTicks(System.TimeSpan.Parse(TimeSpan.Execute()).Ticks * Times.Execute());
    }

    class DateTimeToSubtract : DateTimeToAdd
    {
        public DateTimeToSubtract(IScalarResolver<string> timeSpan, IScalarResolver<int> times)
            : base(timeSpan, times) { }

        public DateTimeToSubtract(IScalarResolver<string> timeSpan)
            : base(timeSpan) { }

        protected override object EvaluateDateTime(DateTime value)
            => value.AddTicks(System.TimeSpan.Parse(TimeSpan.Execute()).Ticks * Times.Execute() * -1);
    }
}
