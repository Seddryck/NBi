using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Projection
{
    class ProjectEngine : IProjectionEngine
    {
        public IEnumerable<IColumnIdentifier> Identifiers { get; }

        public ProjectEngine(ProjectArgs args)
            => Identifiers = args.Identifiers;

        public IResultSet Execute(IResultSet resultSet)
        {
            var columns = new List<DataColumn>();
            foreach (var identifier in Identifiers)
            {
                switch (identifier)
                {
                    case ColumnOrdinalIdentifier id:
                        if (id.Ordinal < resultSet.Columns.Count)
                            columns.Add(resultSet.Columns[id.Ordinal]);
                        break;
                    case ColumnNameIdentifier id:
                        if (resultSet.Columns.Contains(id.Name))
                            columns.Add(resultSet.Columns[id.Name]);
                        break;
                    default: throw new ArgumentException();
                }
            }

            var i = 0;
            while (i < resultSet.Columns.Count)
            {
                if (IsColumnToRemove(resultSet.Columns[i], columns))
                    resultSet.Columns.RemoveAt(i);
                else
                    i++;
            }

            int moved = 0;
            for (int j = 0; j < columns.Count; j++)
            {
                if (columns[j].Ordinal>=moved)
                {
                    columns[j].SetOrdinal(moved);
                    moved += 1;
                }
            }
            resultSet.AcceptChanges();
            return resultSet;
        }

        protected virtual bool IsColumnToRemove(DataColumn dataColumn, IEnumerable<DataColumn> columns)
            => !columns.Contains(dataColumn);
    }
}
