using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Lookup
{
    public class ColumnMapping
    {
        public string ChildColumn { get; }
        public string ParentColumn { get; }
        public ColumnType Type { get; }

        public ColumnMapping(string childColumn, string parentColumn, ColumnType type)
        {
            ChildColumn = childColumn;
            ParentColumn = parentColumn;
            Type = type;
        }

        public IColumnDefinition ToColumnDefinition(Func<string> target)
        {
            var defColumn = new Column()
            {
                Role = ColumnRole.Key,
                Type = Type,
            };
            if (target().StartsWith("#"))
                defColumn.Index = Convert.ToInt32(target().Substring(1));
            else
                defColumn.Name = target();
            return defColumn;
        }
    }
}
