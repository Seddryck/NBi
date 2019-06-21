using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Strategy
{
    public class ReplaceByDefaultStrategy : IMissingValueStrategy
    {
        private decimal DefaultValue { get; }

        public ReplaceByDefaultStrategy(decimal defaultValue) => DefaultValue = defaultValue;

        public IEnumerable<object> Execute(IEnumerable<object> values)
        {
            var caster = new NumericCaster();
            return values.Select(x => caster.IsStrictlyValid(x) ? caster.Execute(x) : DefaultValue).Cast<object>();
        }
    }
}
