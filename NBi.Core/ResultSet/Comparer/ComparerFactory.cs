using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet.Comparer
{
    class ComparerFactory
    {
        public BaseComparer Get(ColumnType type)
        {
            switch (type)
            {
                case ColumnType.Text:
                    return new TextComparer();
                case ColumnType.Numeric:
                    return new NumericComparer();
                case ColumnType.DateTime:
                    return new DateTimeComparer();
                case ColumnType.Boolean:
                    return new BooleanComparer();
            }
            throw new ArgumentException();
        }
    }
}
