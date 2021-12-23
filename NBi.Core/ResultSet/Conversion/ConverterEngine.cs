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
            var identifier = new ColumnIdentifierFactory().Instantiate(column);
            
            var columnTemp = rs.AddColumn(Guid.NewGuid().ToString(), converter.DestinationType);
            foreach(var row in rs.Rows)
                row[columnTemp.Name] = converter.Execute(row[identifier]) ?? DBNull.Value;

            rs.GetColumn(identifier).ReplaceBy(columnTemp);
            return rs;
        }
    }
}
