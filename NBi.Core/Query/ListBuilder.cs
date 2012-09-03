using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NBi.Core.Query
{
    public class ListBuilder
    {
        public virtual IList<string> Build(IDbCommand cmd)
        {
            var qe = new QueryEngineFactory().GetExecutor(cmd);
            var ds = qe.Execute();
            var list = Load(ds);
            return list;
        }

        protected List<string> Load(DataSet dataSet)
        {
            return Load(dataSet.Tables[0]);
        }

        protected List<string> Load(DataTable table)
        {
            var list = new List<string>();
            foreach (DataRow row in table.Rows)
                list.Add((string)row[0]);

            return list;
        }
    }
}
