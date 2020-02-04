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
            switch (columnType)
            {
                case ColumnType.Text: return new ReturnDefaultStrategy(string.Empty);
                case ColumnType.Numeric: return new ReturnDefaultStrategy(0);
                case ColumnType.DateTime: return new ReturnDefaultStrategy(new DateTime(1900, 1, 1));
                case ColumnType.Boolean: return new ReturnDefaultStrategy(false);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public object Execute() => DefaultValue;
    }
}
