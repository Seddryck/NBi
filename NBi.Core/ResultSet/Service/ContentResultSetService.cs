using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Service
{
    class ContentResultSetService : ListRowResultSetService
    {
        private readonly IEnumerable<string> columnNames;

        public ContentResultSetService(IContent content)
            : base(content.Rows)
        {
            columnNames = content.Columns;
        }

        public override ResultSet Execute()
        {
            var rs = base.Execute();
            for (int i = 0; i < columnNames.Count(); i++)
            {
                if (!string.IsNullOrEmpty(columnNames.ElementAt(i)))
                    rs.Table.Columns[i].ColumnName = columnNames.ElementAt(i);
            }
            return rs;
        }
    }
}
