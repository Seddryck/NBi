using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
    abstract class AbstractNumericTransformation : INativeTransformation
    {
        public AbstractNumericTransformation()
        { }

        public object Evaluate(object value)
        {
            switch (value)
            {
                case null: return EvaluateNull();
                case DBNull dbnull: return EvaluateNull();
                case decimal numeric: return EvaluateNumeric(numeric);
                default: return EvaluateUncasted(value);
            }
        }

        private object EvaluateUncasted(object value)
        {
            if (value as string == "(null)")
                EvaluateNull();

            var caster = new NumericCaster();
            var numeric = caster.Execute(value);
            return EvaluateNumeric(numeric);
        }

        protected virtual object EvaluateNull() => null;
        protected abstract decimal EvaluateNumeric(decimal numeric);
    }

    class NullToZero : AbstractNumericTransformation
    {
        protected override object EvaluateNull() => 0;
        protected override decimal EvaluateNumeric(decimal numeric) => numeric;
    }

    class NumericToCeiling : AbstractNumericTransformation
    {
        protected override decimal EvaluateNumeric(decimal numeric) => Math.Ceiling(numeric);
    }

    class NumericToFloor : AbstractNumericTransformation
    {
        protected override decimal EvaluateNumeric(decimal numeric) => Math.Floor(numeric);
    }

    class NumericToInteger : AbstractNumericTransformation
    {
        protected override decimal EvaluateNumeric(decimal numeric) => Math.Round(numeric, 0);
    }

    class NumericToRound : AbstractNumericTransformation
    {
        public int Digits { get; }

        public NumericToRound(string digits)
        {
            var caster = new NumericCaster();
            var d = caster.Execute(digits);
            Digits = Math.Truncate(d) == d ? Convert.ToInt32(Math.Truncate(d)) : throw new ArgumentException();
        }

        protected override decimal EvaluateNumeric(decimal numeric) => Math.Round(numeric, Digits);
    }

    class NumericToClip : AbstractNumericTransformation
    {
        public decimal Min { get; }
        public decimal Max { get; }

        public NumericToClip(string min, string max)
        {
            var caster = new NumericCaster();
            Min = caster.Execute(min);
            Max = caster.Execute(max);
        }

        protected override decimal EvaluateNumeric(decimal numeric) => (numeric < Min) ? Min : (numeric > Max) ? Max : numeric;
    }
}
