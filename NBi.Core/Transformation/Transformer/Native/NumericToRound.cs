using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer.Native
{
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
}
