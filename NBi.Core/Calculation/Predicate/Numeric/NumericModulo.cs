using NBi.Core.Scalar.Caster;
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

        public NumericModulo(bool not, object secondOperand, object reference) 
            : base(not, reference)
        {
            this.secondOperand = secondOperand;
        }
        protected override bool Compare(decimal x, decimal y)
        {
            var caster = new NumericCaster();
            var z = caster.Execute(secondOperand);
            return x % z == y;
        }

        public override string ToString()
        {
            return $"modulo {secondOperand} is equal to {Reference}";
        }
    }
}
