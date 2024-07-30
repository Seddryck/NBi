using NBi.Core.ResultSet;
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
        private ColumnType ColumnType { get; }

        public FailureMissingValueStrategy(ColumnType columnType)
            => ColumnType = columnType;

        public IEnumerable<object> Execute(IEnumerable<object?> values)
        {
            var caster = new CasterFactory().Instantiate(ColumnType);
            if (values.All(x => ((NumericCaster)caster).IsStrictlyValid(x)))
                return values.Select(x => caster.Execute(x)).Cast<object>();
            else
                throw new ArgumentException();
        }
    }
}
