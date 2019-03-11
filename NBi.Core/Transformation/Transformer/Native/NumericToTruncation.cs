using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    abstract class AbstractNumericToTruncation : INativeTransformation
    {
        public AbstractNumericToTruncation()
        { }

        public object Evaluate(object value)
        {
            if (value == null || DBNull.Value.Equals(value) || value as string == "(empty)" || value as string == "(null)" || value is string && string.IsNullOrEmpty(value as string))
                return null;
            else
            {
                var caster = new NumericCaster();
                var numeric = caster.Execute(value);
                return Truncate(numeric);
            }
        }

        protected abstract decimal Truncate(decimal numeric);
    }

    class NumericToCeiling : AbstractNumericToTruncation
    {
        protected override decimal Truncate(decimal numeric) => Math.Ceiling(numeric);
    }

    class NumericToFloor : AbstractNumericToTruncation
    {
        protected override decimal Truncate(decimal numeric) => Math.Floor(numeric);
    }

    class NumericToInteger : AbstractNumericToTruncation
    {
        protected override decimal Truncate(decimal numeric) => Math.Round(numeric, 0);
    }

    class NumericToRound : AbstractNumericToTruncation
    {
        public int Digits { get; }

        public NumericToRound(string digits)
        {
            var caster = new NumericCaster();
            var d = caster.Execute(digits);
            Digits = Math.Truncate(d) == d ? Convert.ToInt32(Math.Truncate(d)) : throw new ArgumentException();
        }

        protected override decimal Truncate(decimal numeric) => Math.Round(numeric, Digits);
    }

    class NumericToClip : AbstractNumericToTruncation
    {
        public decimal Min { get; }
        public decimal Max { get; }

        public NumericToClip(string min, string max)
        {
            var caster = new NumericCaster();
            Min = caster.Execute(min);
            Max = caster.Execute(max);
        }

        protected override decimal Truncate(decimal numeric) => (numeric < Min) ? Min : (numeric > Max) ? Max : numeric;
    }
}
