using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Strategy
{
    public class DropStrategy : IMissingValueStrategy
    {
        public IEnumerable<object> Execute(IEnumerable<object> values)
        {
            var caster = new NumericCaster();
            return values.Where(x => caster.IsValid(x)).Select(x => caster.Execute(x)).Cast<object>(); 
        }
    }
}
