using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility
{
    public interface IResultSet
    {
        DataTable Table { get; }

        DataColumnCollection Columns { get; }

        DataRowCollection Rows { get; }
        void Load(DataTable table);
        void Load(IEnumerable<DataRow> rows);
        void AddRange(IEnumerable<DataRow> rows);
        IResultSet Clone();
    }
}
