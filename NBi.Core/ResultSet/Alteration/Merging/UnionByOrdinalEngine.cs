using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Merging
{
    public class UnionByOrdinalEngine : IMergingEngine
    {
        public IResultSetService ResultSetResolver { get; }

        public UnionByOrdinalEngine(IResultSetService resultSetResolver)
            => (ResultSetResolver) = (resultSetResolver);

        public IResultSet Execute(IResultSet rs)
        {
            var secondRs = ResultSetResolver.Execute();

            //Add new columns to the original result-set
            for (int i = rs.Columns.Count; i < secondRs.Columns.Count; i++)
                rs.Columns.Add(new DataColumn(secondRs.Columns[i].ColumnName, typeof(object)) { DefaultValue = DBNull.Value });

            //Add new columns to the second result-set
            for (int i = secondRs.Columns.Count; i < rs.Columns.Count; i++)
                secondRs.Columns.Add(new DataColumn(rs.Columns[i].ColumnName, typeof(object)) { DefaultValue = DBNull.Value });

            //Import each row of the second dataset
            foreach (DataRow row in secondRs.Rows)
                rs.Table.ImportRow(row);
            rs.Table.AcceptChanges();

            return rs;
        }
    }
}
