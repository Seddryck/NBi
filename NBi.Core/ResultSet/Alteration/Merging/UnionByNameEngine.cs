using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Merging
{
    public class UnionByNameEngine : IMergingEngine
    {
        public IResultSetService ResultSetResolver { get; }

        public UnionByNameEngine(IResultSetService resultSetResolver)
            => (ResultSetResolver) = (resultSetResolver);

        public IResultSet Execute(IResultSet rs)
        {
            var secondRs = ResultSetResolver.Execute();

            //Add new columns to the original result-set
            var existingColumns = rs.Columns.Cast<DataColumn>().Select(x => x.ColumnName);
            foreach (DataColumn dataColumn in secondRs.Columns)
                if (!existingColumns.Contains(dataColumn.ColumnName))
                    rs.Columns.Add(new DataColumn(dataColumn.ColumnName, typeof(object)) { DefaultValue = DBNull.Value });

            //Reorder and add not-existing column to the second result-set
            foreach (DataColumn dataColumn in rs.Columns)
            { 
                if (!secondRs.Columns.Cast<DataColumn>().Any(x => x.ColumnName == dataColumn.ColumnName))
                    secondRs.Columns.Add(new DataColumn(dataColumn.ColumnName, typeof(object)) { DefaultValue = DBNull.Value });
                secondRs.Columns[dataColumn.ColumnName].SetOrdinal(dataColumn.Ordinal);
            }

            //Import each row of the second dataset
            foreach (var row in secondRs.Rows)
                rs.Add(row);
            rs.AcceptChanges();

            return rs;
        }
    }
}
