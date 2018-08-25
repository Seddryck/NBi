using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet
{
    class ColumnNameIdentifier : IColumnIdentifier
    {
        public string Name { get; private set; }
        public string Label => $"[{Name}]";

        public ColumnNameIdentifier(string name)
        {
            Name = name;
        }
    }
}
