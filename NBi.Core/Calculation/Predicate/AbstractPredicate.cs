using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate
{
    abstract class AbstractPredicate : IPredicate
    {
        private readonly bool not;

        public bool Execute(object x) => not ? !Apply(x) : Apply(x);

        protected abstract bool Apply(object x);
        
        public AbstractPredicate(bool not)
        {
            this.not = not;
        }
    }
}
