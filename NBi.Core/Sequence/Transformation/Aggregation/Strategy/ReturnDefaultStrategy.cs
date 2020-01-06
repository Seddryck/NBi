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
        internal ReturnDefaultStrategy(ColumnType columnType)
            : this(DefaultValueByColumnType(columnType)) { }

        private static object DefaultValueByColumnType(ColumnType columnType)
        {
            switch (columnType)
            {
                case ColumnType.Text: return string.Empty;
                case ColumnType.Numeric: return 0;
                case ColumnType.DateTime: return new DateTime(1900, 1, 1);
                case ColumnType.Boolean: return false;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public object Execute() => DefaultValue;
    }
}
