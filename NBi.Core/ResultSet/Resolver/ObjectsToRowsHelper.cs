using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver
{
    class ObjectsToRowsHelper
    {
        public IEnumerable<IRow> Execute(IEnumerable<object> objects)
        {
            var rows = new List<IRow>();
            foreach (var obj in objects)
            {
                var row = new Row();
                if (obj is IEnumerable<object> items)
                    foreach (var item in items)
                    {
                        var cell = new Cell() { Value = item?.ToString() };
                        row.Cells.Add(cell);
                    }
                rows.Add(row);
            }

            return rows;
        }


        private class Row : IRow
        {
            private readonly IList<ICell> cells = new List<ICell>();
            public IList<ICell> Cells { get => cells; }
        }

        private class Cell : ICell
        {
            public string Value { get; set; }
            public string ColumnName { get; set; }
        }
    }
}
