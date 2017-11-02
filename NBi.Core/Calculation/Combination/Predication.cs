using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Combination
{
    class Predication
    {
        public IPredicate Predicate { get; private set; }
        public string Operand { get; private set; }
        public Predication(IPredicate predicate, string operand)
        {
            this.Predicate = predicate;
            this.Operand = operand;
        }
    }
}
