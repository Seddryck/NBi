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
            for (int i = rs.ColumnCount; i < secondRs.ColumnCount; i++)
                rs.AddColumn(secondRs.GetColumn(i).Name);

            //Add new columns to the second result-set
            for (int i = secondRs.ColumnCount; i < rs.ColumnCount; i++)
                secondRs.AddColumn(rs.GetColumn(i).Name);

            //Import each row of the second dataset
            foreach (var row in secondRs.Rows)
                rs.AddRow(row.ItemArray);
            rs.AcceptChanges();

            return rs;
        }
    }
}
