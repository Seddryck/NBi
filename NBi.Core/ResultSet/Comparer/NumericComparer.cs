using System;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Comparer
{
    class NumericComparer : BaseComparer
    {
        public ComparerResult Compare(object x, object y, string tolerance)
        {

            return base.Compare(x, y, ToleranceFactory.BuildNumeric(tolerance));
        }
        
        public ComparerResult Compare(object x, object y, decimal tolerance)
        {
            return base.Compare(x, y, new NumericAbsoluteTolerance(tolerance));
        }

        protected override ComparerResult CompareObjects(object x, object y)
        {
            var builder = new IntervalBuilder(x);
            builder.Build();
            if (builder.IsValid())
                return CompareDecimals
                    (
                        builder.GetInterval()
                        , Convert.ToDecimal(y, NumberFormatInfo.InvariantInfo)
                    ); 

            builder = new IntervalBuilder(y);
            builder.Build();
            if (builder.IsValid())
                return CompareDecimals
                    (
                        builder.GetInterval()
                        , Convert.ToDecimal(x, NumberFormatInfo.InvariantInfo)
                    ); 
            
            return CompareObjects(x, y, new NumericAbsoluteTolerance(0));
        }

        protected override ComparerResult CompareObjects(object x, object y, Rounding rounding)
        {
            if (!(rounding is NumericRounding))
                throw new ArgumentException("Rounding must be of type 'NumericRounding'");

            return CompareObjects(x, y, (NumericRounding)rounding);
        }

        protected override ComparerResult CompareObjects(object x, object y, Tolerance tolerance)
        {
            if (!(tolerance is NumericTolerance))
                throw new ArgumentException("Tolerance must be of type 'NumericTolerance'");

            return CompareObjects(x, y, (NumericTolerance)tolerance);
        }
        
        public ComparerResult CompareObjects(object x, object y, NumericRounding rounding)
        {
            var rxDecimal = Convert.ToDecimal(x, NumberFormatInfo.InvariantInfo);
            var ryDecimal = Convert.ToDecimal(y, NumberFormatInfo.InvariantInfo);

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
                        , Convert.ToDecimal(y, NumberFormatInfo.InvariantInfo)
                    ); 

            builder = new IntervalBuilder(y);
            builder.Build();
            if (builder.IsValid())
                return CompareDecimals
                    (
                        builder.GetInterval()
                        , Convert.ToDecimal(x, NumberFormatInfo.InvariantInfo)
                    ); 

            var rxDecimal = Convert.ToDecimal(x, NumberFormatInfo.InvariantInfo);
            var ryDecimal = Convert.ToDecimal(y, NumberFormatInfo.InvariantInfo);

            return CompareDecimals(rxDecimal, ryDecimal, tolerance);               
        }

        protected ComparerResult CompareDecimals(decimal x, decimal y, NumericTolerance tolerance)
        {
            if (tolerance is NumericAbsoluteTolerance)
                return CompareDecimals(x, y, (NumericAbsoluteTolerance)tolerance);

            if (tolerance is NumericPercentageTolerance)
                return CompareDecimals(x, y, (NumericPercentageTolerance)tolerance);

            throw new ArgumentException();
        }

        protected ComparerResult CompareDecimals(decimal x, decimal y, NumericAbsoluteTolerance tolerance)
        {
            //Compare decimals (with tolerance)
            if (IsEqual(x, y, tolerance.Value))
                return ComparerResult.Equality;

            return new ComparerResult(x.ToString(NumberFormatInfo.InvariantInfo));
        }

        protected ComparerResult CompareDecimals(decimal x, decimal y, NumericPercentageTolerance tolerance)
        {
            //Compare decimals (with tolerance)
            if (IsEqual(x, y, x * tolerance.Value))
                return ComparerResult.Equality;

            return new ComparerResult(x.ToString(NumberFormatInfo.InvariantInfo));
        }

        protected ComparerResult CompareDecimals(Interval interval, decimal y)
        {
            if (interval.Contains(y))
                return ComparerResult.Equality;

            return new ComparerResult(interval.ToString());
        }

        protected bool IsEqual(decimal x, decimal y, decimal tolerance)
        {
            //Console.WriteLine("IsEqual: {0} {1} {2} {3} {4} {5}", x, y, tolerance, Math.Abs(x - y), x == y, Math.Abs(x - y) <= tolerance);

            //quick check
            if (x == y)
                return true;

            //Stop checks if tolerance is set to 0
            if (tolerance == 0)
                return false;

            //include some math[Time consumming] (Tolerance needed to validate)
            return (Math.Abs(x - y) <= tolerance);
        }


        protected override bool IsValidObject(object x)
        {
            return (IsValidNumeric(x));
        }

    }
}
