using NBi.Core.Scalar.Casting;
using System;
using System.Globalization;
using System.Linq;
using NBi.Core.Scalar.Interval;

namespace NBi.Core.Scalar.Comparer
{
    class NumericComparer : BaseComparer
    {
        private readonly ICaster<decimal> caster;

        public NumericComparer()
        {
            caster = new NumericCaster();
        }

        public ComparerResult Compare(object? x, object? y, string tolerance)
        {
            return base.Compare(x, y, new NumericToleranceFactory().Instantiate(tolerance));
        }

        internal ComparerResult Compare(object? x, object? y, decimal tolerance, SideTolerance side)
        {
            return base.Compare(x, y, new NumericAbsoluteTolerance(tolerance, side));
        }

        protected override ComparerResult CompareObjects(object? x, object? y)
        {
            if (x is not null)
            {
                var builder = new NumericIntervalBuilder(x);
                builder.Build();
                if (builder.IsValid())
                    return CompareDecimals
                        (
                            builder.GetInterval()
                            , caster.Execute(y)
                        );
            }

            if (y is not null)
            {
                var builder = new NumericIntervalBuilder(y);
                builder.Build();
                if (builder.IsValid())
                    return CompareDecimals
                        (
                            builder.GetInterval()
                            , caster.Execute(x)
                        );
            }
            return CompareObjects(x, y, NumericAbsoluteTolerance.None);
        }

        protected override ComparerResult CompareObjects(object? x, object? y, Rounding rounding)
        {
            if (rounding is not NumericRounding)
                throw new ArgumentException("Rounding must be of type 'NumericRounding'");

            return CompareObjects(x, y, (NumericRounding)rounding);
        }

        protected override ComparerResult CompareObjects(object? x, object? y, Tolerance tolerance)
        {
            if (tolerance is not NumericTolerance)
                throw new ArgumentException("Tolerance must be of type 'NumericTolerance'");

            return CompareObjects(x, y, (NumericTolerance)tolerance);
        }

        public ComparerResult CompareObjects(object? x, object? y, NumericRounding rounding)
        {
            var rxDecimal = caster.Execute(x);
            var ryDecimal = caster.Execute(y);

            rxDecimal = rounding.GetValue(rxDecimal);
            ryDecimal = rounding.GetValue(ryDecimal);

            return CompareObjects(rxDecimal, ryDecimal);
        }

        protected ComparerResult CompareObjects(object? x, object? y, NumericTolerance tolerance)
        {
            if (x is not null)
            {
                var builder = new NumericIntervalBuilder(x);
                builder.Build();
                if (builder.IsValid())
                    return CompareDecimals
                        (
                            builder.GetInterval()
                            , caster.Execute(y)
                        );
            }

            if (y is not null)
            {
                var builder = new NumericIntervalBuilder(y);
                builder.Build();
                if (builder.IsValid())
                    return CompareDecimals
                        (
                            builder.GetInterval()
                            , caster.Execute(x)
                        );
            }
            var rxDecimal = caster.Execute(x);
            var ryDecimal = caster.Execute(y);

            return CompareDecimals(rxDecimal, ryDecimal, tolerance);
        }

        protected ComparerResult CompareDecimals(decimal expected, decimal actual, NumericTolerance tolerance)
            => tolerance switch
            {
                NumericAbsoluteTolerance t => CompareDecimals(expected, actual, t),
                NumericPercentageTolerance t => CompareDecimals(expected, actual, t),
                NumericBoundedPercentageTolerance t => CompareDecimals(expected, actual, t),
                _ => throw new ArgumentException()
            };

        protected ComparerResult CompareDecimals(decimal expected, decimal actual, NumericAbsoluteTolerance tolerance)
        {
            //Compare decimals (with tolerance)
            if (IsEqual(expected, actual, tolerance.Value, tolerance.Side))
                return ComparerResult.Equality;

            return new ComparerResult(expected.ToString(NumberFormatInfo.InvariantInfo));
        }

        protected ComparerResult CompareDecimals(decimal expected, decimal actual, NumericPercentageTolerance tolerance)
        {
            //Compare decimals (with tolerance)
            if (IsEqual(expected, actual, expected * tolerance.Value, tolerance.Side))
                return ComparerResult.Equality;

            return new ComparerResult(expected.ToString(NumberFormatInfo.InvariantInfo));
        }

        protected ComparerResult CompareDecimals(decimal expected, decimal actual, NumericBoundedPercentageTolerance tolerance)
        {
            //Compare decimals (with bounded tolerance)
            if (IsEqual(expected, actual, tolerance.GetValue(expected), tolerance.Side))
                return ComparerResult.Equality;

            return new ComparerResult(expected.ToString(NumberFormatInfo.InvariantInfo));
        }

        protected ComparerResult CompareDecimals(NumericInterval interval, decimal actual)
        {
            if (interval.Contains(actual))
                return ComparerResult.Equality;

            return new ComparerResult(interval.ToString());
        }

        protected bool IsEqual(decimal x, decimal y, decimal tolerance, SideTolerance side)
        {
            //quick check
            if (x == y)
                return true;

            //Stop checks if tolerance is set to 0
            if (tolerance == 0)
                return false;

            //include some math[Time consumming] (Tolerance needed to validate)
            if (Math.Abs(x - y) <= Math.Abs(tolerance))
            {
                return side switch
                {
                    SideTolerance.Both => true,
                    SideTolerance.More => (x <= y),
                    SideTolerance.Less => (x >= y),
                    _ => throw new ArgumentOutOfRangeException(),
                };
            }

            return false;
        }

        protected override bool IsValidObject(object x)
            => new BaseNumericCaster().IsValid(x);
    }
}
