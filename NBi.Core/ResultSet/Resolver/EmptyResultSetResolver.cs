using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver
{
    public class EmptyResultSetResolver : IResultSetResolver
    {
        private EmptyResultSetResolverArgs Args { get; }

        public EmptyResultSetResolver(EmptyResultSetResolverArgs args)
            => Args = args;

        public virtual ResultSet Execute()
        {
            var dataTable = new DataTable();
            if (Args.Identifiers != null)
                foreach (var identifier in Args.Identifiers)
                    dataTable.Columns.Add(new DataColumn(identifier.Name, typeof(object)));

            if (Args.ColumnCount!=null && dataTable.Columns.Count< Args.ColumnCount.Execute())
            {
                var missingColumnCount = Args.ColumnCount.Execute() - dataTable.Columns.Count;
                for (int i = 0; i < missingColumnCount; i++)
                    dataTable.Columns.Add(new DataColumn($"Column_{dataTable.Columns.Count}", typeof(object)));
            }

            var rs = new ResultSet();
            rs.Load(dataTable);
            return rs;
        }
    }
}
