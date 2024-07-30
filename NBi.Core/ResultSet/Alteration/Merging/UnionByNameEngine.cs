using NBi.Extensibility;
using NBi.Extensibility.Resolving;
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
        public IResultSetResolver ResultSetResolver { get; }

        public UnionByNameEngine(IResultSetResolver resultSetResolver)
            => (ResultSetResolver) = (resultSetResolver);

        public IResultSet Execute(IResultSet rs)
        {
            var secondRs = ResultSetResolver.Execute();

            //Add new columns to the original result-set
            var existingColumns = rs.Columns.Select(x => x.Name);
            foreach (var dataColumn in secondRs.Columns)
                if (!existingColumns.Contains(dataColumn.Name))
                    rs.AddColumn(dataColumn.Name);

            //Reorder and add not-existing column to the second result-set
            foreach (var dataColumn in rs.Columns)
            { 
                if (!secondRs.ContainsColumn(dataColumn.Name))
                    secondRs.AddColumn(dataColumn.Name);
                secondRs.GetColumn(dataColumn.Name)?.Move(dataColumn.Ordinal);
            }

            //Import each row of the second dataset
            foreach (var row in secondRs.Rows)
                rs.AddRow(row.ItemArray);
            rs.AcceptChanges();

            return rs;
        }
    }
}
