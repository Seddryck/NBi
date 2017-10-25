using NBi.Core.ResultSet.Converter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Numeric
{
    class NumericModulo : NumericPredicate
    {
        private object secondOperand;

        public NumericModulo(object secondOperand, object reference) 
            : base(reference)
        {
            this.secondOperand = secondOperand;
        }
        protected override bool Compare(decimal x, decimal y)
        {
            var converter = new NumericConverter();
            var z = converter.Convert(secondOperand);
            return x % z == y;
        }

        public override string ToString()
        {
            return $"modulo {secondOperand} is equal to {Reference}";
        }
    }
}
