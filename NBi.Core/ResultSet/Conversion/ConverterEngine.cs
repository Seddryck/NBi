using NBi.Core.Scalar.Conversion;
using NBi.Extensibility;
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
        private readonly string column;
        private readonly IConverter converter;

        public ConverterEngine(string column, IConverter converter)
        {
            this.column = !string.IsNullOrEmpty(column) ? column : throw new ArgumentException("The column can't be empty. You should specify the name of the column or the index preceded by a #.", nameof(column));
            this.converter = converter ?? throw new ArgumentNullException("The converter can't be null.", nameof(converter));
        }

        public IResultSet Execute(IResultSet rs)
        {
            var columnName = column.StartsWith("#") ? rs.Columns[Convert.ToInt32(column.Replace("#", ""))].ColumnName : column;
            var columnIndex = column.StartsWith("#") ? Convert.ToInt32(column.Replace("#", "")) : rs.Columns[columnName].Ordinal;
            var columnNameTemp = columnName + "__temp";

            rs.Columns.Add(new DataColumn(columnNameTemp, converter.DestinationType));
            rs.Columns[columnNameTemp].SetOrdinal(columnIndex + 1);

            foreach(DataRow row in rs.Rows)
                row[columnNameTemp] = converter.Execute(row[columnIndex]) ?? DBNull.Value;

            rs.Columns.RemoveAt(columnIndex);
            rs.Columns[columnNameTemp].ColumnName = columnName;
            return rs;
        }
    }
}
