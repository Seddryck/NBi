using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Strategy
{
    public class FailureMissingValueStrategy : IMissingValueStrategy
    {
        public IEnumerable<object> Execute(IEnumerable<object> values)
        {
            var caster = new NumericCaster();
            if (values.All(x => caster.IsStrictlyValid(x)))
                return values.Select(x => caster.Execute(x)).Cast<object>();
            else
                throw new ArgumentException();
        }
    }
}
