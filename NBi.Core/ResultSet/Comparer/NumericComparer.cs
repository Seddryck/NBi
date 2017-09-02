using NBi.Core.ResultSet.Converter;
using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Comparer
{
    class NumericComparer : BaseComparer
    {
        private readonly IConverter<decimal> converter;

        public NumericComparer()
        {
            converter = new NumericConverter();
        }

        public ComparerResult Compare(object x, object y, string tolerance)
        {
            return base.Compare(x, y, new NumericToleranceFactory().Instantiate(tolerance));
        }
        
        internal ComparerResult Compare(object x, object y, decimal tolerance, SideTolerance side)
        {
            return base.Compare(x, y, new NumericAbsoluteTolerance(tolerance, side));
        }

        protected override ComparerResult CompareObjects(object x, object y)
        {
            var builder = new IntervalBuilder(x);
            builder.Build();
            if (builder.IsValid())
                return CompareDecimals
                    (
                        builder.GetInterval()
                        , converter.Convert(y)
                    ); 

            builder = new IntervalBuilder(y);
            builder.Build();
            if (builder.IsValid())
                return CompareDecimals
                    (
                        builder.GetInterval()
                        , converter.Convert(x)
                    ); 
            
            return CompareObjects(x, y, NumericAbsoluteTolerance.None);
        }

        protected override ComparerResult CompareObjects(object x, object y, Rounding rounding)
        {
            if (!(rounding is NumericRounding))
                throw new ArgumentException("Rounding must be of type 'NumericRounding'");

            return CompareObjects(x, y, (NumericRounding)rounding);
        }

        protected override ComparerResult CompareObjects(object x, object y, Tolerance tolerance)
        {
            if (tolerance == null)
                tolerance = NumericAbsoluteTolerance.None;

            if (!(tolerance is NumericTolerance))
                throw new ArgumentException("Tolerance must be of type 'NumericTolerance'");

            return CompareObjects(x, y, (NumericTolerance)tolerance);
        }
        
        public ComparerResult CompareObjects(object x, object y, NumericRounding rounding)
        {
            var rxDecimal = converter.Convert(x);
            var ryDecimal = converter.Convert(y);

            rxDecimal = rounding.GetValue(rxDecimal);
            ryDecimal = rounding.GetValue(ryDecimal);

            return CompareObjects(rxDecimal, ryDecimal);
        }

        protected ComparerResult CompareObjects(object x, object y, NumericTolerance tolerance)
        {
            var builder = new IntervalBuilder(x);
            builder.Build();
            if (builder.IsValid())
                return CompareDecimals
                    (
                        builder.GetInterval()
                        , converter.Convert(y)
                    ); 

            builder = new IntervalBuilder(y);
            builder.Build();
            if (builder.IsValid())
                return CompareDecimals
                    (
                        builder.GetInterval()
                        , converter.Convert(x)
                    ); 

            var rxDecimal = converter.Convert(x);
            var ryDecimal = converter.Convert(y);

            return CompareDecimals(rxDecimal, ryDecimal, tolerance);               
        }

        protected ComparerResult CompareDecimals(decimal expected, decimal actual, NumericTolerance tolerance)
        {
            if (tolerance is NumericAbsoluteTolerance)
                return CompareDecimals(expected, actual, (NumericAbsoluteTolerance)tolerance);

            if (tolerance is NumericPercentageTolerance)
                return CompareDecimals(expected, actual, (NumericPercentageTolerance)tolerance);

            if (tolerance is NumericBoundedPercentageTolerance)
                return CompareDecimals(expected, actual, (NumericBoundedPercentageTolerance)tolerance);

            throw new ArgumentException();
        }

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

        protected ComparerResult CompareDecimals(Interval interval, decimal actual)
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
                switch (side)
                {
                    case SideTolerance.Both:
                        return true;
                    case SideTolerance.More:
                        return (x <= y);
                    case SideTolerance.Less:
                        return (x >= y);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return false;
        }


        protected override bool IsValidObject(object x)
        {
            return new BaseNumericConverter().IsValid(x);
        }

    }
}
