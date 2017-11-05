using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver
{
    class ListRowResultSetResolver : IResultSetResolver
    {
        private readonly IList<IRow> rows;

        public ListRowResultSetResolver(IList<IRow> rows)
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
