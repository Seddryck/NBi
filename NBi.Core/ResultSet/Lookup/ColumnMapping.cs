using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Lookup
{
    public class ColumnMapping
    {
        public IColumnIdentifier CandidateColumn { get; }
        public IColumnIdentifier ReferenceColumn { get; }
        public ColumnType Type { get; }

        public ColumnMapping(IColumnIdentifier candidateColumn, IColumnIdentifier referenceColumn, ColumnType type)
        {
            CandidateColumn = candidateColumn;
            ReferenceColumn = referenceColumn;
            Type = type;
        }

        public ColumnMapping(IColumnIdentifier column, ColumnType type)
            : this(column, column, type)
        { }

        public IColumnDefinition ToColumnDefinition(Func<IColumnIdentifier> target)
        {
            var defColumn = new Column()
            {
                Identifier = target(),
                Role = ColumnRole.Key,
                Type = Type,
            };
            return defColumn;
        }
    }
}
