using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Transformation.Aggregation.Strategy
{
    class ReturnDefaultStrategy : IEmptySeriesStrategy
    {
        private object DefaultValue { get; }

        public ReturnDefaultStrategy(object defaultValue) => DefaultValue = defaultValue;

        internal static ReturnDefaultStrategy Instantiate(ColumnType columnType)
        {
            return columnType switch
            {
                ColumnType.Text => new ReturnDefaultStrategy(string.Empty),
                ColumnType.Numeric => new ReturnDefaultStrategy(0),
                ColumnType.DateTime => new ReturnDefaultStrategy(new DateTime(1900, 1, 1)),
                ColumnType.Boolean => new ReturnDefaultStrategy(false),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public object Execute() => DefaultValue;
    }
}
