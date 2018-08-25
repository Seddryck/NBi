using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet
{
    class ColumnPositionIdentifier : IColumnIdentifier
    {
        public int Position { get; private set; }

        public string Label => $"#{Position.ToString()}";

        public ColumnPositionIdentifier(int position)
        {
            Position = position;
        }
    }
}
