using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Service
{
    class QueryResultSetService : IResultSetService
    {
        private readonly IDbCommand command;

        public QueryResultSetService(IDbCommand cmd)
        {
            command = cmd;
        }

        public virtual ResultSet Execute()
        {
            var qe = new QueryEngineFactory().GetExecutor(command);
            var ds = qe.Execute();
            var rs = new ResultSet();
            rs.Load(ds);
            return rs;
        }
    }
}
