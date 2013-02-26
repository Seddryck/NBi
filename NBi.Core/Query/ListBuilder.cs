using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NBi.Core.Query
{
    /// <summary>
    /// This class build a list of string froma query taking the first cell of each row
    /// </summary>
    public class ListBuilder
    {
        /// <summary>
        /// Execute the command and take the first cell of each row to build an Enumerable of string
        /// </summary>
        /// <param name="cmd">A sql or mdx query to execute</param>
        /// <returns>The first cell of row returned by the query</returns>
        public virtual IEnumerable<string> Build(IDbCommand cmd)
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
