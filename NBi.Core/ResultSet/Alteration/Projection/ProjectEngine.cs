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
            var columns = new List<IResultColumn>();
            foreach (var identifier in Identifiers)
            {
                var column = resultSet.GetColumn(identifier);
                if (column != null) 
                    columns.Add(column);    
            }

            var i = 0;
            while (i < resultSet.ColumnCount)
            {
                var column = resultSet.GetColumn(i) ?? throw new NullReferenceException();
                if (IsColumnToRemove(column, columns))
                    column.Remove();
                else
                    i++;
            }

            int moved = 0;
            for (int j = 0; j < columns.Count; j++)
            {
                if (columns[j].Ordinal>=moved)
                {
                    columns[j].Move(moved);
                    moved += 1;
                }
            }
            resultSet.AcceptChanges();
            return resultSet;
        }

        protected virtual bool IsColumnToRemove(IResultColumn dataColumn, IEnumerable<IResultColumn> columns)
            => !columns.Contains(dataColumn);
    }
}
