using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Service
{
    class ListRowResultSetService : IResultSetService
    {
        private readonly IList<IRow> rows;

        public ListRowResultSetService(IList<IRow> rows)
        {
            this.rows = rows;
        }

        public virtual ResultSet Execute()
        {
            var rs = new ResultSet();
            rs.Load(rows);
            return rs;
        }
    }
}
