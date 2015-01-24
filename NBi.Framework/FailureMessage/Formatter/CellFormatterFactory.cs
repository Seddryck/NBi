using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Unit.Framework.FailureMessage.Formatter
{
    public class CellFormatterFactory
    {
        public CellFormatter GetObject(ColumnType columnType)
        {
            switch (columnType)
            {
                case ColumnType.Text:
                    return new TextCellFormatter();
                case ColumnType.Numeric:
                    return new NumericCellFormatter();
                case ColumnType.DateTime:
                    return new DateTimeCellFormatter();
                case ColumnType.Boolean:
                    return new BooleanCellFormatter();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
