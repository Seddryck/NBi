using NBi.Core.Scalar.Conversion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Conversion
{
    public class ConverterEngine
    {
        public void Execute(ResultSet rs, int columnIndex, IConverter converter)
        {
            var columnName = rs.Columns[columnIndex].ColumnName;
            var columnNameTemp = columnName + "__temp";

            rs.Columns.Add(new DataColumn(columnNameTemp, converter.DestinationType));
            rs.Columns[columnNameTemp].SetOrdinal(columnIndex + 1);

            foreach(DataRow row in rs.Rows)
                row[columnNameTemp] = converter.Execute(row[columnIndex]);

            rs.Columns.RemoveAt(columnIndex);
            rs.Columns[columnNameTemp].ColumnName = columnName;
        }

        public void Execute(ResultSet rs, string columnName, IConverter converter)
        {
            if (!rs.Columns.Contains(columnName))
                throw new ArgumentException( $"The column '{columnName}' doesn't exist!", nameof(columnName));
            else
                Execute(rs, rs.Columns[columnName].Ordinal, converter);
        }

        
    }
}
